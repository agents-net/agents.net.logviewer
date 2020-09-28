using System;
using System.Collections.Generic;
using Agents.Net;
using Agents.Net.LogViewer.Model;

namespace Agents.Net.LogViewer.ViewModel.Messages
{
    public class AgentViewModelCreating : Message
    {
        public AgentViewModelCreating(IEnumerable<LogEntry> relevantEntries, string agentName,
                                      Guid agentId,
                                      Message predecessorMessage)
			: base(predecessorMessage)
        {
            RelevantEntries = relevantEntries;
            AgentName = agentName;
            AgentId = agentId;
        }

        public AgentViewModelCreating(IEnumerable<LogEntry> relevantEntries, string agentName,
                                      Guid agentId,
                                      IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            RelevantEntries = relevantEntries;
            AgentName = agentName;
            AgentId = agentId;
        }
        
        public IEnumerable<LogEntry> RelevantEntries { get; }
        public string AgentName { get; }
        public Guid AgentId { get; }

        protected override string DataToString()
        {
            return $"{nameof(AgentName)}: {AgentName}; {nameof(AgentId)}: {AgentId}";
        }
    }
}
