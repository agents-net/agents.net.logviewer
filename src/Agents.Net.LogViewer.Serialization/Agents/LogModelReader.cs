using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Agents.Net;
using Agents.Net.LogViewer.Serialization.Messages;

namespace Agents.Net.LogViewer.Serialization.Agents
{
    [Consumes(typeof(LogFileOpened))]
    [Produces(typeof(LogEntryRead))]
    public class LogModelReader : Agent
    {
        public LogModelReader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            LogFileOpened opened = messageData.Get<LogFileOpened>();
            using StreamReader reader = new StreamReader(opened.Data, Encoding.UTF8);
            List<Message> messages = new List<Message>();
            int line = 1;
            while (!reader.EndOfStream)
            {
                messages.Add(new LogEntryRead(reader.ReadLine(), messageData, line));
                line++;
            }
            OnMessages(messages);
        }
    }
}
