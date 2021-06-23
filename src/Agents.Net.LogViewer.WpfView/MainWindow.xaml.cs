using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Agents.Net.LogViewer.ViewModel;
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
            
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor
                .FromProperty(ItemsControl.ItemsSourceProperty, typeof(ListBox));
            dpd?.AddValueChanged(MessageLogList, MessageLogListPropertyChangedCallback);
        }

        private void MessageLogListPropertyChangedCallback(object? sender, EventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(MessageLogList.ItemsSource);
            if (view != null)
            {
                view.Filter = MessageFilter;
            }
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

        private bool MessageFilter(object item)
        {
            if(String.IsNullOrEmpty(Filter.Text))
                return true;
            return (((MessageViewModel) item).Name.IndexOf(Filter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void FilterOnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MessageLogList.ItemsSource)?.Refresh();
        }
    }
}