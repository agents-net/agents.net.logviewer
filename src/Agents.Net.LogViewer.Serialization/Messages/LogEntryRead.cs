using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.Serialization.Messages
{
    public class LogEntryRead : Message
    {
        public LogEntryRead(string logData, Message predecessorMessage, int lineNumber)
			: base(predecessorMessage)
        {
            LogData = logData;
            LineNumber = lineNumber;
        }

        public LogEntryRead(string logData, IEnumerable<Message> predecessorMessages, int lineNumber)
			: base(predecessorMessages)
        {
            LogData = logData;
            LineNumber = lineNumber;
        }
        
        public string LogData { get; }
        
        public int LineNumber { get; }

        protected override string DataToString()
        {
            return $"{nameof(LogData)}: {LogData}; {nameof(LineNumber)}: {LineNumber}";
        }
    }
}
