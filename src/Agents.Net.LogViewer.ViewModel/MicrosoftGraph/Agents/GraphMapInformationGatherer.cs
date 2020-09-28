using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(SelectedViewModelChanged))]
    [Consumes(typeof(LogViewModelCreated))]
    [Produces(typeof(IncomingGraphCreating))]
    [Produces(typeof(OutgoingGraphCreating))]
    public class GraphMapInformationGatherer : Agent
    {
        public GraphMapInformationGatherer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //TODO Implement
        }
    }
}
