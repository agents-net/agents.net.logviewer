using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class MessageViewModelCreated : Message
    {
        public MessageViewModelCreated(MessageViewModel viewModel, Message predecessorMessage)
			: base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public MessageViewModelCreated(MessageViewModel viewModel, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public MessageViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
