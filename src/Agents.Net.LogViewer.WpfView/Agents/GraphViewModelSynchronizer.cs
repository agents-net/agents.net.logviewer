using System;
using Agents.Net;
using Agents.Net.LogViewer.WpfView.Messages;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.LogViewer.WpfView.Agents
{
    [Consumes(typeof(MainWindowCreated))]
    [Consumes(typeof(IncomingGraphCreated))]
    [Consumes(typeof(OutgoingGraphCreated))]
    public class GraphViewModelSynchronizer : Agent
    {
        private readonly MessageCollector<MainWindowCreated, IncomingGraphCreated> incomingCollector;
        private readonly MessageCollector<MainWindowCreated, OutgoingGraphCreated> outgoingCollector;
        public GraphViewModelSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            incomingCollector = new MessageCollector<MainWindowCreated, IncomingGraphCreated>(OnMessagesCollected);
            outgoingCollector = new MessageCollector<MainWindowCreated, OutgoingGraphCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MainWindowCreated, OutgoingGraphCreated> set)
        {
            set.MarkAsConsumed(set.Message2);
            set.Message1.Window.Dispatcher.Invoke(() => set.Message1.Window.OutgoingGraphViewer.Graph =
                                                            set.Message2.Graph);
        }

        private void OnMessagesCollected(MessageCollection<MainWindowCreated, IncomingGraphCreated> set)
        {
            set.MarkAsConsumed(set.Message2);
            set.Message1.Window.Dispatcher.Invoke(() => set.Message1.Window.IncomingGraphViewer.Graph =
                                                            set.Message2.Graph);
        }

        protected override void ExecuteCore(Message messageData)
        {
            incomingCollector.TryPush(messageData);
            outgoingCollector.TryPush(messageData);
        }
    }
}
