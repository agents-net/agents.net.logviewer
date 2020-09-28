using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(AgentViewModelCreated))]
    [Produces(typeof(AgentsViewModelAggregated))]
    public class AgentsViewModelAggregator : Agent
    {
        private readonly MessageAggregator<AgentViewModelCreated> aggregator;
        public AgentsViewModelAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator =new MessageAggregator<AgentViewModelCreated>(OnAggregated);
        }

        private void OnAggregated(IReadOnlyCollection<AgentViewModelCreated> set)
        {
            MessageDomain.TerminateDomainsOf(set);
            OnMessage(new AgentsViewModelAggregated(set.Select(m => m.ViewModel), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
