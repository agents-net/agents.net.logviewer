using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(SelectedViewModelChanged))]
    [Produces(typeof(IncomingGraphCreating))]
    [Produces(typeof(OutgoingGraphCreating))]
    public class GraphMapInformationGatherer : Agent
    {
        public GraphMapInformationGatherer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            SelectedViewModelChanged selectedViewModelChanged = messageData.Get<SelectedViewModelChanged>();
            OnMessage(new IncomingGraphCreating(selectedViewModelChanged.SelectedViewModel, messageData));
            OnMessage(new OutgoingGraphCreating(selectedViewModelChanged.SelectedViewModel, messageData));
        }
    }
}
