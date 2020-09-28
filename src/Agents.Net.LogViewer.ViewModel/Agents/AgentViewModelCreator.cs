using System;
using System.Collections.Generic;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(AgentViewModelCreating))]
    [Produces(typeof(AgentViewModelCreated))]
    public class AgentViewModelCreator : Agent
    {
        public AgentViewModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            AgentViewModelCreating creating = messageData.Get<AgentViewModelCreating>();
            List<MessageViewModelReference> consumed = new List<MessageViewModelReference>();
            List<MessageViewModelReference> produced = new List<MessageViewModelReference>();
            List<MessageViewModelReference> intercepted = new List<MessageViewModelReference>();
            foreach (LogEntry relevantEntry in creating.RelevantEntries)
            {
                switch (relevantEntry.Log.Type.ToLowerInvariant())
                {
                    case "executing":
                        consumed.Add(new MessageViewModelReference(relevantEntry.Log.Message.Id, relevantEntry.Timestamp));
                        break;
                    case "intercepting":
                        intercepted.Add(new MessageViewModelReference(relevantEntry.Log.Message.Id, relevantEntry.Timestamp));
                        break;
                    case "publishing":
                        produced.Add(new MessageViewModelReference(relevantEntry.Log.Message.Id, relevantEntry.Timestamp));
                        break;
                }
            }

            string name = creating.AgentName;
            if (name.Contains("."))
            {
                name = name.Substring(name.IndexOf('.') + 1);
            }

            AgentViewModel viewModel = new AgentViewModel(name, creating.AgentName,
                                                          creating.AgentId, consumed.ToArray(),
                                                          intercepted.ToArray(), produced.ToArray());
            OnMessage(new AgentViewModelCreated(viewModel, messageData));
        }
    }
}
