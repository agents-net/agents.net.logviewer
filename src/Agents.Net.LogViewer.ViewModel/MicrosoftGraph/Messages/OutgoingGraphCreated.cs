using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class OutgoingGraphCreated : Message
    {
        public OutgoingGraphCreated(Message predecessorMessage)
			: base(predecessorMessage)
        {
        }

        public OutgoingGraphCreated(IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
