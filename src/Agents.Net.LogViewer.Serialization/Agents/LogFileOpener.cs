using System;
using System.IO;
using Agents.Net;
using Agents.Net.LogViewer.Serialization.Messages;

namespace Agents.Net.LogViewer.Serialization.Agents
{
    [Consumes(typeof(LogFileOpening))]
    [Produces(typeof(LogFileOpened))]
    public class LogFileOpener : Agent
    {
        public LogFileOpener(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            LogFileOpening opening = messageData.Get<LogFileOpening>();
            OnMessage(new LogFileOpened(File.OpenRead(opening.Path), messageData));
        }
    }
}
