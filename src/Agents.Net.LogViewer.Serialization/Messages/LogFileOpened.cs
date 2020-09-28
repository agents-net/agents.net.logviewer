using System.Collections.Generic;
using System.IO;
using Agents.Net;

namespace Agents.Net.LogViewer.Serialization.Messages
{
    public class LogFileOpened : Message
    {
        public LogFileOpened(Stream data, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Data = data;
        }

        public LogFileOpened(Stream data, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Data = data;
        }
        
        public Stream Data { get; }

        protected override string DataToString()
        {
            return $"{nameof(Data)}: {Data.Length}";
        }
    }
}
