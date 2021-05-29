using System;

namespace Agents.Net.LogViewer.Model
{
    public class LogEntry
    {
        public LogEntry(DateTime timestamp, AgentLog log, string exception,
                        int lineNumber)
        {
            Timestamp = timestamp;
            Log = log;
            Exception = exception;
            LineNumber = lineNumber;
        }

        public DateTime Timestamp { get; }
        public AgentLog Log { get; }
        public string Exception { get; }
        public int LineNumber { get; }

        public override string ToString()
        {
            return $"{nameof(LineNumber)}: {LineNumber}; {nameof(Timestamp)}: {Timestamp}, {nameof(Log)}: {Log.Type} {Log.Message.Name} with {Log.Agent}, {nameof(Exception)}: {Exception != null}";
        }
    }
}