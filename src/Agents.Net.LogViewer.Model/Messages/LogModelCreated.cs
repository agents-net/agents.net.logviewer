using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.LogViewer.Model.Messages
{
    public class LogModelCreated : Message
    {
        public LogModelCreated(LogModel model, Message predecessorMessage)
			: base(predecessorMessage)
        {
            Model = model;
        }

        public LogModelCreated(LogModel model, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            Model = model;
        }
        
        public LogModel Model { get; }

        protected override string DataToString()
        {
            return $"{nameof(Model)}: {Model}";
        }
    }
}
