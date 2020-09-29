using System;
using System.Collections.Concurrent;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Agents.Net.LogViewer.Model.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(LogModelCreated))]
    [Consumes(typeof(IncomingGraphCreated))]
    [Consumes(typeof(OutgoingGraphCreated))]
    [Produces(typeof(IncomingGraphCreated))]
    [Produces(typeof(OutgoingGraphCreated))]
    [Intercepts(typeof(IncomingGraphCreating))]
    [Intercepts(typeof(OutgoingGraphCreating))]
    public class GraphCache : InterceptorAgent
    {
        private readonly ConcurrentDictionary<BaseViewModel, Graph> incomingCache = new ConcurrentDictionary<BaseViewModel, Graph>();
        private readonly ConcurrentDictionary<BaseViewModel, Graph> outgoingCache = new ConcurrentDictionary<BaseViewModel, Graph>();
        
        public GraphCache(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.Is<LogModelCreated>())
            {
                incomingCache.Clear();
                outgoingCache.Clear();
            }
            else if (messageData.TryGet(out IncomingGraphCreated incomingGraphCreated) &&
                     incomingGraphCreated.TryGetPredecessor(out IncomingGraphCreating incomingGraphCreating))
            {
                incomingCache[incomingGraphCreating.Root] = incomingGraphCreated.Graph;
            }
            else if (messageData.TryGet(out OutgoingGraphCreated outgoingGraphCreated) &&
                     outgoingGraphCreated.TryGetPredecessor(out OutgoingGraphCreating outgoingGraphCreating))
            {
                outgoingCache[outgoingGraphCreating.Root] = outgoingGraphCreated.Graph;
            }
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.TryGet(out IncomingGraphCreating incoming) &&
                incomingCache.ContainsKey(incoming.Root))
            {
                OnMessage(new IncomingGraphCreated(incomingCache[incoming.Root], messageData));
                return InterceptionAction.DoNotPublish;
            }

            if (messageData.TryGet(out OutgoingGraphCreating outgoing) &&
                outgoingCache.ContainsKey(outgoing.Root))
            {
                OnMessage(new OutgoingGraphCreated(outgoingCache[outgoing.Root], messageData));
                return InterceptionAction.DoNotPublish;
            }
            
            return InterceptionAction.Continue;
        }
    }
}
