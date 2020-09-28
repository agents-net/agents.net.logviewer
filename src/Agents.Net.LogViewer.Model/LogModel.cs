using System.Collections.Generic;
using System.Linq;

namespace Agents.Net.LogViewer.Model
{
    public class LogModel
    {
        public LogModel(IEnumerable<LogEntry> logEntries)
        {
            LogEntries = logEntries;
        }

        public IEnumerable<LogEntry> LogEntries { get; }

        public override string ToString()
        {
            return $"{nameof(LogEntries)}: {LogEntries.Count()}";
        }
    }
}