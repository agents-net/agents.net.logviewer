using System;
using Agents.Net.LogViewer.ViewModel;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.WpfView.Messages;

namespace Agents.Net.LogViewer.WpfView.Agents
{
    [Consumes(typeof(MainWindowCreated))]
    [Consumes(typeof(ViewModelSelecting))]
    public class SelectionModifier : Agent
    {
        private readonly MessageCollector<MainWindowCreated, ViewModelSelecting> collector;
        public SelectionModifier(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<MainWindowCreated, ViewModelSelecting>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<MainWindowCreated, ViewModelSelecting> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (set.Message2.ViewModel is MessageViewModel)
            {
                set.Message1.Window.Dispatcher.Invoke(
                    () =>
                    {
                        set.Message1.Window.MessageLogList.ScrollIntoView(set.Message2.ViewModel);
                        set.Message1.Window.MessageLogList.SelectedItem = set.Message2.ViewModel;
                    });
            }
            else
            {
                set.Message1.Window.Dispatcher.Invoke(
                    () =>
                    {
                        set.Message1.Window.AgentList.ScrollIntoView(set.Message2.ViewModel);
                        set.Message1.Window.AgentList.SelectedItem = set.Message2.ViewModel;
                    });
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
