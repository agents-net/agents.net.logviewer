using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.Model.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(LogModelCreated))]
    [Produces(typeof(AgentViewModelCreating))]
    public class AgentsViewModelReader : Agent
    {
        public AgentsViewModelReader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            LogModel model = messageData.Get<LogModelCreated>().Model;
            AgentViewModelCreating[] messages = model.LogEntries
                                                     .Select(e => new
                                                     {
                                                         Definition = (e.Log.Agent, e.Log.AgentId), Entry = e
                                                     })
                                                     .GroupBy(t => t.Definition)
                                                     .Select(g => new AgentViewModelCreating(g.Select(t => t.Entry),
                                                                 g.Key.Agent,
                                                                 g.Key.AgentId,
                                                                 messageData))
                                                     .ToArray();
            OnMessages(messages);
        }
    }
}
