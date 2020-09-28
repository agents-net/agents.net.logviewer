using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.Serialization.Messages
{
    public class LogEntryRead : Message
    {
        public LogEntryRead(string logData, Message predecessorMessage)
			: base(predecessorMessage)
        {
            LogData = logData;
        }

        public LogEntryRead(string logData, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            LogData = logData;
        }
        
        public string LogData { get; }

        protected override string DataToString()
        {
            return $"{nameof(LogData)}: {LogData}";
        }
    }
}
