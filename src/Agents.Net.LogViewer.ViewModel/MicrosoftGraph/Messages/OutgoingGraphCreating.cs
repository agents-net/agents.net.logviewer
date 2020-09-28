using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class OutgoingGraphCreating : Message
    {
        public OutgoingGraphCreating(Message predecessorMessage)
			: base(predecessorMessage)
        {
        }

        public OutgoingGraphCreating(IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
