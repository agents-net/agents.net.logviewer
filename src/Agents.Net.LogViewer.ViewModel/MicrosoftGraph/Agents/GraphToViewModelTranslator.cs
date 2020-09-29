using System;
using Agents.Net;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(GraphNodeDoubleClicked))]
    [Produces(typeof(ViewModelSelecting))]
    public class GraphToViewModelTranslator : Agent
    {
        public GraphToViewModelTranslator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            GraphNodeDoubleClicked doubleClicked = messageData.Get<GraphNodeDoubleClicked>();
            if (doubleClicked.DoubleClickedItem.UserData is BaseViewModel viewModel)
            {
                OnMessage(new ViewModelSelecting(viewModel, messageData));
            }
        }
    }
}
