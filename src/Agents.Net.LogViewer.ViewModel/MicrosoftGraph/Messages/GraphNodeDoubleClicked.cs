using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphNodeDoubleClicked : Message
    {
        public GraphNodeDoubleClicked(Message predecessorMessage)
			: base(predecessorMessage)
        {
        }

        public GraphNodeDoubleClicked(IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
