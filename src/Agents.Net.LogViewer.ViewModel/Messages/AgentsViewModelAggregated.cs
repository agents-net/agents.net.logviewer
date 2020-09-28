using System.Collections.Generic;
using System.Linq;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class AgentsViewModelAggregated : Message
    {
        public AgentsViewModelAggregated(IEnumerable<AgentViewModel> agents, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Agents = agents;
        }

        public AgentsViewModelAggregated(IEnumerable<AgentViewModel> agents, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Agents = agents;
        }
        
        public IEnumerable<AgentViewModel> Agents { get; }

        protected override string DataToString()
        {
            return $"{nameof(Agents)}: {Agents.Count()}";
        }
    }
}
