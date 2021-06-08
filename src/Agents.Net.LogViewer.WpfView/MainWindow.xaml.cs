using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using ModifierKeys = System.Windows.Input.ModifierKeys;

namespace Agents.Net.LogViewer.WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GraphViewer IncomingGraphViewer { get; } = new GraphViewer();
        public GraphViewer OutgoingGraphViewer { get; } = new GraphViewer();
        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            IncomingGraphViewer.BindToPanel(IncomingGraphViewerPanel);
            IncomingGraphViewer.RunLayoutAsync = true;
            OutgoingGraphViewer.BindToPanel(OutgoingGraphViewerPanel);
            OutgoingGraphViewer.RunLayoutAsync = true;
        }

        private void OpenLogOnClick(object sender, RoutedEventArgs e)
        {
            OpenLog();
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key.O:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        OpenLog();
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void OpenLog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Agents log file (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true,
                CheckFileExists = true
            };
            if (openFileDialog.ShowDialog(this) == true)
            {
                OnOpenLogClicked(new OpenLogArgs(openFileDialog.FileName));
            }
        }
        public event EventHandler<OpenLogArgs> OpenLogClicked;

        protected virtual void OnOpenLogClicked(OpenLogArgs e)
        {
            OpenLogClicked?.Invoke(this, e);
        }
    }
}