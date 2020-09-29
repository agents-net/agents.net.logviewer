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
                        Shape = Shape.Ellipse
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
                        Shape = Shape.Ellipse
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
            HashSet<AgentViewModel> processedViewModels = new HashSet<AgentViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(AgentViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = Shape.Ellipse
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (AgentViewModel predecessor in Predecessors(viewModel))
                {
                    Node predecessorNode = AddNode(predecessor);
                    graph.AddEdge(predecessorNode.Id, node.Id);
                }

                return node;
            }

            IEnumerable<AgentViewModel> Predecessors(AgentViewModel viewModel)
            {
                return viewModel.ConsumingMessages.Concat(viewModel.InterceptingMessages)
                                .Where(m => m.ViewModel?.ProducingAgent?.ViewModel != null)
                                .Select(m => m.ViewModel.ProducingAgent.ViewModel)
                                .Distinct();
            }
        }

        private Graph CreateOutgoingAgentGraph(AgentViewModel root)
        {
            Graph graph = new Graph {Attr = {LayerDirection = LayerDirection.TB}};
            HashSet<AgentViewModel> processedViewModels = new HashSet<AgentViewModel>();
            AddNode(root);
            return graph;

            Node AddNode(AgentViewModel viewModel)
            {
                if (!processedViewModels.Add(viewModel))
                {
                    return graph.FindNode(viewModel.Id.ToString("D"));
                }
                Node node = new Node(viewModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = Shape.Ellipse
                    },
                    LabelText = viewModel.Name,
                    UserData = viewModel
                };
                graph.AddNode(node);
                foreach (AgentViewModel successors in Successors(viewModel))
                {
                    Node successorNode = AddNode(successors);
                    graph.AddEdge(node.Id, successorNode.Id);
                }

                return node;
            }

            IEnumerable<AgentViewModel> Successors(AgentViewModel viewModel)
            {
                return viewModel.ProducingMessages
                                .Where(m => m.ViewModel != null)
                                .SelectMany(m => m.ViewModel.UsedBy.Concat(m.ViewModel.InterceptedBy))
                                .Where(a => a.ViewModel != null)
                                .Select(a => a.ViewModel)
                                .Distinct();
            }
        }
    }
}
