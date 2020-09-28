using System;
using System.Windows.Input;

namespace Agents.Net.LogViewer.ViewModel
{
    public class MessageViewModelReference
    {
        public MessageViewModelReference(Guid id, DateTime timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }

        public MessageViewModelReference(Guid id)
        {
            Id = id;
        }

        public MessageViewModelReference(Guid id, MessageViewModel viewModel, DateTime timestamp)
        {
            Id = id;
            ViewModel = viewModel;
            Timestamp = timestamp;
        }

        public Guid Id { get; }
        public MessageViewModel ViewModel { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Timestamp)}: {Timestamp:dd.MM.yy HH:mm:ss.fff}";
        }
    }
}