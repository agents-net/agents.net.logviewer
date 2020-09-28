using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class ViewModelSelecting : Message
    {
        public ViewModelSelecting(BaseViewModel viewModel, Message predecessorMessage)
			: base(predecessorMessage)
        {
            ViewModel = viewModel;
        }

        public ViewModelSelecting(BaseViewModel viewModel, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            ViewModel = viewModel;
        }
        
        public BaseViewModel ViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(ViewModel)}: {ViewModel}";
        }
    }
}
