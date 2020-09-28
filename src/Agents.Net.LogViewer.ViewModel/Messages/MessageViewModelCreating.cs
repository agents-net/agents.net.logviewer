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
            Index = index;
        }

        public MessageViewModelCreating(LogEntry logEntry, int index, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            LogEntry = logEntry;
            Index = index;
        }
        
        public LogEntry LogEntry { get; }
        
        public int Index { get; }

        protected override string DataToString()
        {
            return $"{nameof(LogEntry)}: {LogEntry}; {nameof(Index)}: {Index}";
        }
    }
}
