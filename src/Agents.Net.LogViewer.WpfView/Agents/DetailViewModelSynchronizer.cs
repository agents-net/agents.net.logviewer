using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.WpfView.Messages;

namespace Agents.Net.LogViewer.WpfView.Agents
{
    [Consumes(typeof(SelectedViewModelChanged))]
    [Consumes(typeof(MainWindowCreated))]
    public class DetailViewModelSynchronizer : Agent
    {
        private readonly MessageCollector<MainWindowCreated, SelectedViewModelChanged> collector;
        public DetailViewModelSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<MainWindowCreated, SelectedViewModelChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MainWindowCreated, SelectedViewModelChanged> set)
        {
            set.MarkAsConsumed(set.Message2);
            set.Message1.Window.Dispatcher.Invoke(() =>
            {
                set.Message1.Window.DetailsView.DataContext = set.Message2.SelectedViewModel;
            });
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
