using System;
using System.Windows.Input;

namespace Agents.Net.LogViewer.ViewModel
{
    public class AgentViewModelReference
    {
        public AgentViewModelReference(Guid id, AgentViewModel viewModel, DateTime timestamp)
        {
            Id = id;
            ViewModel = viewModel;
            Timestamp = timestamp;
        }

        public Guid Id { get; }
        public AgentViewModel ViewModel { get; set; }
        public DateTime Timestamp { get; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Timestamp)}: {Timestamp:dd.MM.yy HH:mm:ss.fff}";
        }
    }
}