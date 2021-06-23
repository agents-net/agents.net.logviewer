using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Intercepts(typeof(MessagesViewModelAggregated))]
    [Intercepts(typeof(AgentsViewModelAggregated))]
    public class ViewModelCrossReferencing : InterceptorAgent
    {
        private readonly MessageCollector<MessagesViewModelAggregated, AgentsViewModelAggregated> collector;
        public ViewModelCrossReferencing(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<MessagesViewModelAggregated, AgentsViewModelAggregated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MessagesViewModelAggregated, AgentsViewModelAggregated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            
            AgentReferenceAggregator referenceAggregator = ResolveAgents();
            referenceAggregator.ResolveMessageAgentReferences();
            ResolveMessages();
            
            void ResolveMessages()
            {
                SuccessorCollector successorCollector = new SuccessorCollector();
                foreach (MessageViewModel viewModel in set.Message1.Messages.Values.SelectMany(vm => vm))
                {
                    ResolveMessage(viewModel);
                }

                successorCollector.ResolveSuccessors();

                void ResolveMessage(MessageViewModel viewModel)
                {
                    foreach (MessageViewModelReference predecessor in viewModel.Predecessors)
                    {
                        MessageViewModel predecessorViewModel = set.Message1.Messages[predecessor.Id][0];
                        predecessor.ViewModel = predecessorViewModel;
                        predecessor.Timestamp = viewModel.Timestamp;
                        successorCollector.AddSuccessor(predecessorViewModel,
                                                        new MessageViewModelReference(viewModel.Id,
                                                            viewModel,
                                                            viewModel.Timestamp));
                    }

                    if (viewModel.Child != null)
                    {
                        MessageViewModel childViewModel = set.Message1.Messages[viewModel.Child.Id][0];
                        viewModel.Child.ViewModel = childViewModel;
                        viewModel.Child.Timestamp = viewModel.Timestamp;
                    }

                    viewModel.ProducingAgent.ViewModel ??= set.Message2.Agents
                                                              .FirstOrDefault(a => a.Id == 
                                                                                  viewModel.ProducingAgent.Id);
                }
            }
            
            AgentReferenceAggregator ResolveAgents()
            {
                AgentReferenceAggregator aggregator = new AgentReferenceAggregator();
                foreach (AgentViewModel agent in set.Message2.Agents)
                {
                    foreach (MessageViewModelReference reference in agent.ConsumingMessages)
                    {
                        MessageViewModel viewModel = set.Message1.Messages[reference.Id][0];
                        aggregator.AddConsumed(viewModel,new AgentViewModelReference(agent.Id,agent,reference.Timestamp));
                        reference.ViewModel = viewModel;
                    }
                    foreach (MessageViewModelReference reference in agent.InterceptingMessages)
                    {
                        MessageViewModel viewModel = set.Message1.Messages[reference.Id][0];
                        aggregator.AddIntercepted(viewModel,new AgentViewModelReference(agent.Id,agent,reference.Timestamp));
                        reference.ViewModel = viewModel;
                    }
                    foreach (MessageViewModelReference reference in agent.ProducingMessages)
                    {
                        MessageViewModel viewModel = set.Message1.Messages[reference.Id][0];
                        reference.ViewModel = viewModel;
                        viewModel.ProducingAgent = new AgentViewModelReference(agent.Id, agent, reference.Timestamp);
                    }
                }

                return aggregator;
            }
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            collector.Push(messageData);
            return InterceptionAction.Continue;
        }

        private class SuccessorCollector
        {
            private Dictionary<MessageViewModel, List<MessageViewModelReference>> successors = new Dictionary<MessageViewModel, List<MessageViewModelReference>>();

            public void AddSuccessor(MessageViewModel viewModel, MessageViewModelReference successorReference)
            {
                if (!successors.ContainsKey(viewModel))
                {
                    successors.Add(viewModel, new List<MessageViewModelReference>());
                }
                successors[viewModel].Add(successorReference);
            }

            public void ResolveSuccessors()
            {
                foreach (KeyValuePair<MessageViewModel,List<MessageViewModelReference>> keyValuePair in successors)
                {
                    keyValuePair.Key.Successors = keyValuePair.Value.ToArray();
                }
            }
        }
        
        private class AgentReferenceAggregator
        {
            private Dictionary<MessageViewModel, AgentReferenceCollector> collectors = new Dictionary<MessageViewModel, AgentReferenceCollector>();

            public void ResolveMessageAgentReferences()
            {
                foreach (KeyValuePair<MessageViewModel,AgentReferenceCollector> keyValuePair in collectors)
                {
                    keyValuePair.Value.ResolveMessageAgentReferences(keyValuePair.Key);
                }
            }

            public void AddConsumed(MessageViewModel messageViewModel, AgentViewModelReference reference)
            {
                if (!collectors.ContainsKey(messageViewModel))
                {
                    collectors.Add(messageViewModel, new AgentReferenceCollector());
                }

                collectors[messageViewModel].AddConsumed(reference);
            }

            public void AddIntercepted(MessageViewModel messageViewModel, AgentViewModelReference reference)
            {
                if (!collectors.ContainsKey(messageViewModel))
                {
                    collectors.Add(messageViewModel, new AgentReferenceCollector());
                }

                collectors[messageViewModel].AddIntercepted(reference);
            }
            
            private class AgentReferenceCollector
            {
                private readonly List<AgentViewModelReference> consumed = new List<AgentViewModelReference>();
                private readonly List<AgentViewModelReference> intercepted = new List<AgentViewModelReference>();

                public void AddConsumed(AgentViewModelReference reference)
                {
                    consumed.Add(reference);
                }

                public void AddIntercepted(AgentViewModelReference reference)
                {
                    intercepted.Add(reference);
                }

                public void ResolveMessageAgentReferences(MessageViewModel messageViewModel)
                {
                    messageViewModel.UsedBy = consumed.ToArray();
                    messageViewModel.InterceptedBy = intercepted.ToArray();
                }
            }
        }
    }
}
