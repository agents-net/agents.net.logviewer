using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class MessagesViewModelAggregated : Message
    {
        public MessagesViewModelAggregated(Dictionary<Guid, MessageViewModel[]> messages, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Messages = messages;
        }

        public MessagesViewModelAggregated(Dictionary<Guid, MessageViewModel[]> messages,
                                           IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Messages = messages;
        }
        
        public Dictionary<Guid, MessageViewModel[]> Messages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Messages)}: {Messages.Count()}";
        }
    }
}
