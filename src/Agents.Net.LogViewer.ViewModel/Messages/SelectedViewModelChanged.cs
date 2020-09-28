using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class SelectedViewModelChanged : Message
    {

        public SelectedViewModelChanged(BaseViewModel selectedViewModel, Message predecessorMessage)
			: base(predecessorMessage)
        {
            SelectedViewModel = selectedViewModel;
        }

        public SelectedViewModelChanged(BaseViewModel selectedViewModel, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            SelectedViewModel = selectedViewModel;
        }
        
        public BaseViewModel SelectedViewModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(SelectedViewModel)}: {SelectedViewModel}";
        }
    }
}
