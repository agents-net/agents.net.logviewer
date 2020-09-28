using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Agents.Net.LogViewer.WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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