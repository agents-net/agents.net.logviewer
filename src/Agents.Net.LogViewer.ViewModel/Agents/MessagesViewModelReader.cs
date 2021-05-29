using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.Model.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(LogModelCreated))]
    [Produces(typeof(MessageViewModelCreating))]
    public class MessagesViewModelReader : Agent
    {
        public MessagesViewModelReader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //problem: child messages that are not previously published
            //idea1: log all child messages as also published (double or more publishes)
            //**idea2: log child messages completely and add them here
            //idea3: do not allow that (how - thats stupid)
            LogModel model = messageData.Get<LogModelCreated>().Model;
            LogEntry[] publishedMessages = model.LogEntries
                                                .Where(e => e.Log.Type.Equals("publishing",
                                                                              StringComparison.OrdinalIgnoreCase))
                                                .ToArray();
            Guid[] publishedIds = publishedMessages.Select(m => m.Log.Message.Id)
                                                   .ToArray();
            Guid[] silentMessages = model.LogEntries
                                              .SelectMany(e => e.Log.Message.Predecessors)
                                              .Concat(model.LogEntries
                                                           .Where(e => e.Log.Type.Equals("executing",
                                                                      StringComparison.OrdinalIgnoreCase))
                                                           .Select(l => l.Log.Message.Id))
                                              .Distinct()
                                              .Where(id => publishedMessages.All(m => m.Log.Message.Id != id))
                                              .ToArray();
            LogEntry[] publishedAsChild = publishedMessages
                                          .SelectMany(l => l.Log.Message.Children().Select(c => new {log = l, child = c}))
                                          .Where(l => !publishedIds.Contains(l.child.Id))
                                          .GroupBy(l => l.child.Id)
                                          .Select(g => g.First())
                                          .Select(l => new LogEntry(l.log.Timestamp,
                                                                    new AgentLog(l.log.Log.Agent,
                                                                        l.log.Log.Type,
                                                                        l.log.Log.AgentId,
                                                                        l.child),
                                                                    l.log.Exception,
                                                      l.log.LineNumber))
                                          .ToArray();
            publishedIds = publishedIds.Concat(publishedAsChild.Select(p => p.Log.Message.Id))
                                       .ToArray();
            SilentDecorator[] silentDecorators = model.LogEntries.SelectMany(l => l.Log.Message.Children())
                                                      .Concat(model.LogEntries.Select(l => l.Log.Message))
                                                      .Distinct(new MessageLogEqualityComparer())
                                                      .Where(m => !publishedIds.Contains(m.Id))
                                                      .Select(m => SilentDecorator.Create(m, publishedIds))
                                                      .Where(s => s.IsSilentDecorator)
                                                      .ToArray();
            LogEntry[] silentDecoratorPseudoPublisher = silentDecorators.Select(d => new
                                                                      {
                                                                          decorator = d,
                                                                          publishers = GetPossiblePublishers(d)
                                                                      })
                                                                      .Select(p => new
                                                                      {
                                                                          p.decorator,
                                                                          publisher = SelectBestGuessPublisher(p.publishers, p.decorator)
                                                                      })
                                                                      .Select(p => new LogEntry(
                                                                                  p.publisher.Timestamp,
                                                                                  new AgentLog(p.publisher.Log.Agent,
                                                                                      p.publisher.Log.Type,
                                                                                      p.publisher.Log.AgentId,
                                                                                      p.decorator.Decorator),
                                                                                  p.publisher.Exception,
                                                                                  p.publisher.LineNumber))
                                                                      .ToArray();
            silentMessages = silentMessages.Except(silentDecoratorPseudoPublisher.Select(p => p.Log.Message.Id))
                                           .Except(publishedAsChild.Select(p => p.Log.Message.Id))
                                           .ToArray();
            IEnumerable<LogEntry> externalMessages = silentMessages
                                                            .Select(id => model.LogEntries.FirstOrDefault(e => e.Log.Message.Id == id))
                                                            .Where(e => e != null)
                                                            .Select(e => new LogEntry(e.Timestamp,
                                                                                      new AgentLog("Extern", "Publishing", Guid.Empty,
                                                                                          e.Log.Message), e.Exception,
                                                                        e.LineNumber));
            
            MessageViewModelCreating[] messages = publishedMessages.Concat(externalMessages)
                                                                   .Concat(publishedAsChild)
                                                                   .Concat(silentDecoratorPseudoPublisher)
                                                                   .OrderBy(l => l.Timestamp)
                                                                   .Select((t, i) => new MessageViewModelCreating(t, i+1, messageData))
                                                                   .ToArray();
            OnMessages(messages);
            
            IEnumerable<LogEntry> GetPossiblePublishers(SilentDecorator silentDecorator)
            {
                return model.LogEntries.SkipWhile(l => !l.Log.Type.Equals("publishing",
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                                       l.Log.Message.Id != silentDecorator.PublishedChild.Id)
                            .Skip(1)
                            .TakeWhile(l => l.Log.Message.Id != silentDecorator.Decorator.Id &&
                                            l.Log.Message.Children().All(c => c.Id != silentDecorator.Decorator.Id))
                            .Where(l => l.Log.Message.Id == silentDecorator.PublishedChild.Id ||
                                        l.Log.Message.Children().Any(c => c.Id == silentDecorator.PublishedChild.Id));
            }
            
            LogEntry SelectBestGuessPublisher(IEnumerable<LogEntry> publishers, SilentDecorator silentDecorator)
            {
                return publishers.OrderByDescending(p => p.Log.Type.Equals("publishing",
                                                                           StringComparison.OrdinalIgnoreCase))
                                 .ThenBy(
                                     p => GetDamerauLevenshteinDistance(p.Log.Agent, silentDecorator.Decorator.Name))
                                 .First();
            }
        }

        private class SilentDecorator
        {
            private readonly MessageLog decorator;
            private readonly MessageLog publishedChild;

            private SilentDecorator(MessageLog decorator, MessageLog publishedChild)
            {
                this.decorator = decorator;
                this.publishedChild = publishedChild;
            }

            public MessageLog Decorator => decorator;

            public MessageLog PublishedChild => publishedChild;

            public bool IsSilentDecorator => publishedChild != null;

            public static SilentDecorator Create(MessageLog potentialDecorator, Guid[] publishedMessages)
            {
                MessageLog child = potentialDecorator.Child;
                MessageLog publishedChild = null;
                while (child != null)
                {
                    if (publishedMessages.Contains(child.Id))
                    {
                        publishedChild = child;
                        break;
                    }
                    child = child.Child;
                }

                return new SilentDecorator(potentialDecorator, publishedChild);
            }
        }
        
        private class MessageLogEqualityComparer : IEqualityComparer<MessageLog>
        {
            public bool Equals(MessageLog x, MessageLog y)
            {
                return Equals(x?.Id, y?.Id);
            }

            public int GetHashCode(MessageLog obj)
            {
                return obj.Id.GetHashCode();
            }
        }
        
        public static int GetDamerauLevenshteinDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };
 
            int[,] matrix = new int[bounds.Height, bounds.Width];
 
            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };
 
            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;
 
                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));
 
                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }
 
                    matrix[height, width] = distance;
                }
            }
 
            return matrix[bounds.Height - 1, bounds.Width - 1];
        }
    }
}
