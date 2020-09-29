using System;

namespace Agents.Net.LogViewer.ViewModel
{
    public class MessageViewModel : BaseViewModel
    {
        public MessageViewModel(string name, string fullName, Guid id,
                 int index, DateTime timestamp, string data, string exception,
                 MessageViewModelReference[] predecessors, MessageViewModelReference child,
                 AgentViewModelReference producingAgent)
            : base(name, fullName, id)
        {
            Index = index;
            Timestamp = timestamp;
            Data = data;
            Exception = exception;
            Predecessors = predecessors;
            Child = child;
            ProducingAgent = producingAgent;
        }

        public int Index { get; }
        public AgentViewModelReference ProducingAgent { get; set; }
        public DateTime Timestamp { get; }
        public string Data { get; }
        public string Exception { get; }
        public MessageViewModelReference[] Predecessors { get; }

        public MessageViewModelReference[] Successors { get; set; } = Array.Empty<MessageViewModelReference>();
        public MessageViewModelReference Child { get; }

        public AgentViewModelReference[] InterceptedBy { get; set; } = Array.Empty<AgentViewModelReference>();
        public AgentViewModelReference[] UsedBy { get; set; } = Array.Empty<AgentViewModelReference>();
    }
}