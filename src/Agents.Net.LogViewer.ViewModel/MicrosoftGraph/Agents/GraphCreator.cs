using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(IncomingGraphCreating))]
    [Consumes(typeof(OutgoingGraphCreating))]
    [Produces(typeof(IncomingGraphCreated))]
    [Produces(typeof(OutgoingGraphCreated))]
    public class GraphCreator : Agent
    {
        public GraphCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //TODO Implement
        }
    }
}
