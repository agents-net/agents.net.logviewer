using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Agents.Net.LogViewer.ViewModel.Annotations;

namespace Agents.Net.LogViewer.ViewModel
{
    public class AgentViewModel : BaseViewModel
    {
        public AgentViewModel(string name, string fullName, Guid id,
                              MessageViewModelReference[] consumingMessages, MessageViewModelReference[] interceptingMessages,
                              MessageViewModelReference[] producingMessages)
            : base(name, fullName, id)
        {
            ConsumingMessages = consumingMessages;
            InterceptingMessages = interceptingMessages;
            ProducingMessages = producingMessages;
        }

        public MessageViewModelReference[] ConsumingMessages { get; }
        public MessageViewModelReference[] InterceptingMessages { get; }
        public MessageViewModelReference[] ProducingMessages { get; }
    }
}