using System.Collections.Generic;

namespace Agents.Net.LogViewer.Model
{
    public static class ModelExtensions
    {
        public static IEnumerable<MessageLog> Children(this MessageLog messageLog)
        {
            List<MessageLog> result = new List<MessageLog>();
            MessageLog child = messageLog.Child;
            while (child != null)
            {
                result.Add(child);
                child = child.Child;
            }

            return result;
        }
    }
}