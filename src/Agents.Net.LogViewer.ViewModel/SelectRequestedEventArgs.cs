using System;

namespace Agents.Net.LogViewer.ViewModel
{
    public class SelectRequestedEventArgs : EventArgs
    {
        public SelectRequestedEventArgs(BaseViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public BaseViewModel ViewModel { get; }
    }
}