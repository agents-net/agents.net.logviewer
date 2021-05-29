using System.Collections.Generic;
using Agents.Net;
using Agents.Net.LogViewer.Model;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class MessageViewModelCreating : Message
    {
        public MessageViewModelCreating(LogEntry logEntry, int index, Message predecessorMessage)
			: base(predecessorMessage)
        {
            LogEntry = logEntry;
        }

        public MessageViewModelCreating(LogEntry logEntry, int index, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            LogEntry = logEntry;
        }
        
        public LogEntry LogEntry { get; }

        protected override string DataToString()
        {
            return $"{nameof(LogEntry)}: {LogEntry}";
        }
    }
}
