using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.Serialization.Messages
{
    public class LogFileOpening : Message
    {
        public LogFileOpening(string path, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Path = path;
        }

        public LogFileOpening(string path, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Path = path;
        }
        
        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Path)}: {Path}";
        }
    }
}
