using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class IncomingGraphCreated : Message
    {
        public IncomingGraphCreated(Message predecessorMessage)
			: base(predecessorMessage)
        {
        }

        public IncomingGraphCreated(IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
