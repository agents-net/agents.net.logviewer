using System;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.Model.Messages;
using Agents.Net.LogViewer.Serialization.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Agents.Net.LogViewer.Serialization.Agents
{
    [Consumes(typeof(LogEntryRead))]
    [Produces(typeof(LogEntryModelCreated))]
    public class LogEntryModelParser : Agent
    {
        public LogEntryModelParser(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            LogEntryRead entryRead = messageData.Get<LogEntryRead>();
            try
            {
                JObject log = JObject.Parse(entryRead.LogData);
                if (!log.ContainsKey("@t") ||
                    log["@t"]?.Value<DateTime>() == null ||
                    !log.ContainsKey("log") ||
                    !(log["log"] is JObject agentLog) ||
                    !agentLog.ContainsKey("$type") ||
                    agentLog["$type"]?.Value<string>() != "AgentLog")
                {
                    OnMessage(new LogEntryModelCreated(null, messageData));
                    return;
                }

                DateTime timestamp = log["@t"]?.Value<DateTime>() ?? default;
                JsonSerializer serializer = new JsonSerializer();
                AgentLog deserializedLog = serializer.Deserialize<AgentLog>(agentLog.CreateReader());
                string exception = null;
                if (log.ContainsKey("@x"))
                {
                    exception = log["@x"]?.Value<string>();
                }

                OnMessage(new LogEntryModelCreated(new LogEntry(timestamp, deserializedLog, exception, entryRead.LineNumber), messageData));
            }
            catch (JsonException)
            {
                OnMessage(new LogEntryModelCreated(null, messageData));
            }
        }
    }
}
