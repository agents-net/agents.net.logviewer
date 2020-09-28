using System;
using System.Linq;
using Agents.Net;
using Agents.Net.LogViewer.Model.Messages;
using Agents.Net.LogViewer.ViewModel.Annotations;
using Agents.Net.LogViewer.ViewModel.Messages;

namespace Agents.Net.LogViewer.ViewModel.Agents
{
    [Consumes(typeof(MessagesViewModelAggregated))]
    [Consumes(typeof(AgentsViewModelAggregated))]
    [Produces(typeof(LogViewModelCreated))]
    public class LogViewModelCreator : Agent
    {
        private readonly MessageCollector<MessagesViewModelAggregated, AgentsViewModelAggregated> collector;
        public LogViewModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<MessagesViewModelAggregated, AgentsViewModelAggregated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MessagesViewModelAggregated, AgentsViewModelAggregated> set)
        {
            OnMessage(new LogViewModelCreated(new LogViewModel(set.Message1.Messages.Values.SelectMany(vm => vm).OrderBy(vm => vm.Timestamp), set.Message2.Agents.OrderBy(a => a.Name)), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
