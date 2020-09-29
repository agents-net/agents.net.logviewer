using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class IncomingGraphCreating : Message
    {
        public IncomingGraphCreating(BaseViewModel root, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Root = root;
        }

        public IncomingGraphCreating(BaseViewModel root, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Root = root;
        }
        
        public BaseViewModel Root { get; }

        protected override string DataToString()
        {
            return $"{nameof(Root)}: {Root}";
        }
    }
}
