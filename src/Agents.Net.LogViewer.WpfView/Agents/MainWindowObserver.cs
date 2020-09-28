using System;
using System.Windows.Controls;
using Agents.Net;
using Agents.Net.LogViewer.Serialization.Messages;
using Agents.Net.LogViewer.ViewModel;
using Agents.Net.LogViewer.ViewModel.Messages;
using Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages;
using Agents.Net.LogViewer.WpfView.Messages;

namespace Agents.Net.LogViewer.WpfView.Agents
{
    [Consumes(typeof(MainWindowCreated))]
    [Produces(typeof(LogFileOpening))]
    [Produces(typeof(SelectedViewModelChanged))]
    [Produces(typeof(GraphNodeDoubleClicked))]
    public class MainWindowObserver : Agent, IDisposable
    {
        private MainWindow mainWindow;
        private Message message;
        public MainWindowObserver(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            message = messageData;
            mainWindow = messageData.Get<MainWindowCreated>().Window;
            mainWindow.OpenLogClicked += MainWindowOnOpenLogClicked;
            mainWindow.MessageLogList.SelectionChanged += MessageLogListOnSelectionChanged;
            mainWindow.AgentList.SelectionChanged += MessageLogListOnSelectionChanged;
        }

        private void MessageLogListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem == null)
                {
                    return;
                }
                if (listBox == mainWindow.MessageLogList)
                {
                    mainWindow.AgentList.SelectedItem = null;
                }
                else
                {
                    mainWindow.MessageLogList.SelectedItem = null;
                }
                OnMessage(new SelectedViewModelChanged((BaseViewModel) listBox.SelectedItem, message));
            }
        }

        private void MainWindowOnOpenLogClicked(object sender, OpenLogArgs e)
        {
            OnMessage(new LogFileOpening(e.FileName, message));
        }

        public void Dispose()
        {
            if (mainWindow != null)
            {
                mainWindow.MessageLogList.SelectionChanged -= MessageLogListOnSelectionChanged;
                mainWindow.AgentList.SelectionChanged -= MessageLogListOnSelectionChanged;
            }
        }
    }
}
