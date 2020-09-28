using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Agents.Net.LogViewer.ViewModel
{
    public class LogViewModel
    {
        public ObservableCollection<MessageViewModel> Messages { get; }
        public ObservableCollection<AgentViewModel> Agents { get; }

        public LogViewModel(IEnumerable<MessageViewModel> messageViewModels, IEnumerable<AgentViewModel> agentViewModels)
        {
            Messages = new ObservableCollection<MessageViewModel>(messageViewModels);
            Agents = new ObservableCollection<AgentViewModel>(agentViewModels);
        }

        public override string ToString()
        {
            return $"{nameof(Messages)}: {Messages.Count}, {nameof(Agents)}: {Agents.Count}";
        }
    }
}