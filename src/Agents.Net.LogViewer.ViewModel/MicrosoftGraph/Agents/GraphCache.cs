using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Agents.Net.LogViewer.Model.Messages;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(LogModelCreated))]
    [Produces(typeof(IncomingGraphCreated))]
    [Produces(typeof(OutgoingGraphCreated))]
    [Intercepts(typeof(IncomingGraphCreating))]
    [Intercepts(typeof(OutgoingGraphCreating))]
    public class GraphCache : InterceptorAgent
    {
        public GraphCache(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            //TODO Implement
            return InterceptionAction.Continue;
        }
    }
}
