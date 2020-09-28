using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.Model.Messages
{
    public class LogEntryModelCreated : Message
    {
        public LogEntryModelCreated(LogEntry log, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Log = log;
        }

        public LogEntryModelCreated(LogEntry log, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Log = log;
        }
        
        public LogEntry Log { get; set; }

        protected override string DataToString()
        {
            return $"{nameof(Log)}: {Log}";
        }
    }
}
