using System;
using System.Threading;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(SelectedViewModelChanged))]
    [Produces(typeof(ViewModelSelecting))]
    public sealed class DetailViewModelWatcher : Agent, IDisposable
    {
        private BaseViewModel activeViewModel;
        private SelectedViewModelChanged lastMessage;
        public DetailViewModelWatcher(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            SelectedViewModelChanged viewModelChanged = messageData.Get<SelectedViewModelChanged>();
            lastMessage = viewModelChanged;
            viewModelChanged.SelectedViewModel.SelectRequested += ActiveViewModelOnSelectRequested;
            BaseViewModel oldViewModel = Interlocked.Exchange(ref activeViewModel, viewModelChanged.SelectedViewModel);
            if (oldViewModel != null)
            {
                oldViewModel.SelectRequested -= ActiveViewModelOnSelectRequested;
            }
        }

        public void Dispose()
        {
            if (activeViewModel != null)
            {
                activeViewModel.SelectRequested -= ActiveViewModelOnSelectRequested;
            }
        }

        private void ActiveViewModelOnSelectRequested(object sender, SelectRequestedEventArgs e)
        {
            OnMessage(new ViewModelSelecting(e.ViewModel, lastMessage));
        }
    }
}
