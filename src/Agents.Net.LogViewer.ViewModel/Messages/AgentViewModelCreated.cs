using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class AgentViewModelCreated : Message
    {
        public AgentViewModelCreated(AgentViewModel viewModel, Message predecessorMessage)
			: base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public AgentViewModelCreated(AgentViewModel viewModel, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public AgentViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
