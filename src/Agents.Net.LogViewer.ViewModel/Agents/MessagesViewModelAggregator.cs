using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(MessageViewModelCreated))]
    [Produces(typeof(MessagesViewModelAggregated))]
    public class MessagesViewModelAggregator : Agent
    {
        private readonly MessageAggregator<MessageViewModelCreated> aggregator;
        public MessagesViewModelAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator =new MessageAggregator<MessageViewModelCreated>(OnAggregated);
        }

        private void OnAggregated(IReadOnlyCollection<MessageViewModelCreated> set)
        {
            MessageDomain.TerminateDomainsOf(set);
            OnMessage(new MessagesViewModelAggregated(set.GroupBy(m => m.ViewModel.Id).ToDictionary(g => g.Key, g => g.Select(m => m.ViewModel).ToArray()), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
