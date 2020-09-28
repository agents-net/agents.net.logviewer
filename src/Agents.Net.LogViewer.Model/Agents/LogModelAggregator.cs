using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.Model.Messages;

namespace Agents.Net.LogViewer.Model.Agents
{
    [Consumes(typeof(LogEntryModelCreated))]
    [Produces(typeof(LogModelCreated))]
    public class LogModelAggregator : Agent
    {
        private readonly MessageAggregator<LogEntryModelCreated> aggregator;
        
        public LogModelAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator = new MessageAggregator<LogEntryModelCreated>(OnAggregated);
        }

        private void OnAggregated(IReadOnlyCollection<LogEntryModelCreated> set)
        {
            MessageDomain.TerminateDomainsOf(set);
            LogEntry[] entries = set.Where(m => m.Log != null)
                                    .Select(m => m.Log)
                                    .OrderBy(l => l.Timestamp)
                                    .ToArray();
            OnMessage(new LogModelCreated(new LogModel(entries), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
