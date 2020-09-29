using System;
using System.Collections.Generic;
using Agents.Net;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class IncomingGraphCreated : Message
    {

        public IncomingGraphCreated(Graph graph, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Graph = graph;
        }

        public IncomingGraphCreated(Graph graph, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Graph = graph;
        }
        
        public Graph Graph { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
