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
        public GraphViewModelSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //TODO Implement
        }
    }
}
