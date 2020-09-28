using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.WpfView.Messages;

namespace Agents.Net.LogViewer.WpfView.Agents
{
    [Consumes(typeof(LogViewModelCreated))]
    [Consumes(typeof(MainWindowCreated))]
    public class ViewModelSynchronizer : Agent
    {
        private readonly MessageCollector<LogViewModelCreated, MainWindowCreated> collector;
        public ViewModelSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<LogViewModelCreated, MainWindowCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<LogViewModelCreated, MainWindowCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.Message2.Window.Dispatcher.Invoke(() =>
            {
                set.Message2.Window.Overview.DataContext = set.Message1.ViewModel;
            });
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
