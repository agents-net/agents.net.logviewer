using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(MessageViewModelCreating))]
    [Produces(typeof(MessageViewModelCreated))]
    public class MessageViewModelCreator : Agent
    {
        public MessageViewModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            MessageViewModelCreating creating = messageData.Get<MessageViewModelCreating>();
            string name = creating.LogEntry.Log.Message.Name;
            if (name.Contains("."))
            {
                name = name.Substring(name.IndexOf('.') + 1);
            }

            MessageViewModel messageViewModel = new MessageViewModel(name, creating.LogEntry.Log.Message.Name,
                                                                     creating.LogEntry.Log.Message.Id, creating.Index,
                                                                     creating.LogEntry.Timestamp,
                                                                     creating.LogEntry.Log.Message.Data,
                                                                     creating.LogEntry.Exception,
                                                                     creating.LogEntry.Log.Message.Predecessors
                                                                             .Select(
                                                                                 p => new MessageViewModelReference(p))
                                                                             .ToArray(),
                                                                     creating.LogEntry.Log.Message.Child != null
                                                                         ? new MessageViewModelReference(
                                                                             creating.LogEntry.Log.Message.Child.Id)
                                                                         : null,
                                                                     new AgentViewModelReference(
                                                                         creating.LogEntry.Log.AgentId, null,
                                                                         creating.LogEntry.Timestamp)
            );
            OnMessage(new MessageViewModelCreated(messageViewModel, messageData));
        }
    }
}
