using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class LogViewModelCreated : Message
    {
        public LogViewModelCreated(LogViewModel viewModel, Message predecessorMessage)
			: base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public LogViewModelCreated(LogViewModel viewModel, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public LogViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
