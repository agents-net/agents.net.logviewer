using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class IncomingGraphCreating : Message
    {
        public IncomingGraphCreating(Message predecessorMessage)
			: base(predecessorMessage)
        {
        }

        public IncomingGraphCreating(IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
