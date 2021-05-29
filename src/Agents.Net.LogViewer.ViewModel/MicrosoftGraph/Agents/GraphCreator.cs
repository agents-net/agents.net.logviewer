using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(IncomingGraphCreating))]
    [Consumes(typeof(OutgoingGraphCreating))]
    [Produces(typeof(IncomingGraphCreated))]
    [Produces(typeof(OutgoingGraphCreated))]
    public class GraphCreator : Agent
    {
        public GraphCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out IncomingGraphCreating incomingGraphCreating))
            {
                OnMessage(new IncomingGraphCreated(incomingGraphCreating.Root is MessageViewModel messageViewModel
                                                       ? CreateIncomingMessageGraph(messageViewModel)
                                                       : CreateIncomingAgentGraph(
                                                           (AgentViewModel) incomingGraphCreating.Root),
                                                   messageData));
            }
            else
            {
                OutgoingGraphCreating outgoingGraphCreating = messageData.Get<OutgoingGraphCreating>();
                OnMessage(new OutgoingGraphCreated(outgoingGraphCreating.Root is MessageViewModel messageViewModel
                                                       ? CreateOutgoingMessageGraph(messageViewModel)
                                                       : CreateOutgoingAgentGraph(
                                                           (AgentViewModel) outgoingGraphCreating.Root),
                                                   messageData));
            }
        }

        private Graph CreateIncomingMessageGraph(MessageViewModel root)
        {
            Graph graph = new Graph {Attr = {LayerDirection = LayerDirection.TB}};
            HashSet<MessageViewModel> processedViewModels = new HashSet<MessageViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(MessageViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = Shape.Box
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (MessageViewModel predecessor in viewModel.Predecessors.Select(p => p.ViewModel)
                                                                  .Where(vm => vm != null))
                {
                    Node predecessorNode = AddNode(predecessor);
                    graph.AddEdge(predecessorNode.Id, node.Id);
                }

                return node;
            }
        }

        private Graph CreateOutgoingMessageGraph(MessageViewModel root)
        {
            Graph graph = new Graph {Attr = {LayerDirection = LayerDirection.TB}};
            HashSet<MessageViewModel> processedViewModels = new HashSet<MessageViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(MessageViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = Shape.Box
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (MessageViewModel successor in viewModel.Successors.Select(p => p.ViewModel)
                                                                  .Where(vm => vm != null))
                {
                    Node successorNode = AddNode(successor);
                    graph.AddEdge(node.Id, successorNode.Id);
                }

                return node;
            }
        }

        private Graph CreateIncomingAgentGraph(AgentViewModel root)
        {
            Graph graph = new Graph {Attr = {LayerDirection = LayerDirection.TB}};
            HashSet<BaseViewModel> processedViewModels = new HashSet<BaseViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(BaseViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = viewModel is AgentViewModel ? Shape.Ellipse : Shape.Box
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (BaseViewModel predecessor in Predecessors(viewModel))
                {
                    Node predecessorNode = AddNode(predecessor);
                    graph.AddEdge(predecessorNode.Id, node.Id);
                }

                return node;
            }

            IEnumerable<BaseViewModel> Predecessors(BaseViewModel viewModel)
            {
                if (viewModel is AgentViewModel agentViewModel)
                {
                    return agentViewModel.ConsumingMessages.Concat(agentViewModel.InterceptingMessages)
                                         .Where(m => m.ViewModel != null)
                                         .Select(m => m.ViewModel)
                                         .Distinct();
                }

                MessageViewModel messageViewModel = (MessageViewModel) viewModel;
                return messageViewModel.ProducingAgent?.ViewModel != null
                           ? new[] {messageViewModel.ProducingAgent?.ViewModel}
                           : Enumerable.Empty<BaseViewModel>();
            }
        }

        private Graph CreateOutgoingAgentGraph(AgentViewModel root)
        {
            Graph graph = new Graph {Attr = {LayerDirection = LayerDirection.TB}};
            HashSet<BaseViewModel> processedViewModels = new HashSet<BaseViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(BaseViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = viewModel is AgentViewModel ? Shape.Ellipse : Shape.Box
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (BaseViewModel successors in Successors(viewModel))
                {
                    Node successorNode = AddNode(successors);
                    graph.AddEdge(node.Id, successorNode.Id);
                }

                return node;
            }

            IEnumerable<BaseViewModel> Successors(BaseViewModel viewModel)
            {
                if (viewModel is AgentViewModel agentViewModel)
                {
                    return agentViewModel.ProducingMessages
                                         .Where(m => m.ViewModel != null)
                                         .Select(m => m.ViewModel)
                                         .Distinct();
                }

                MessageViewModel messageViewModel = (MessageViewModel) viewModel;
                return messageViewModel.UsedBy.Concat(messageViewModel.InterceptedBy)
                                       .Where(a => a.ViewModel != null)
                                       .Select(a => a.ViewModel)
                                       .Distinct();
            }
        }
    }
}
