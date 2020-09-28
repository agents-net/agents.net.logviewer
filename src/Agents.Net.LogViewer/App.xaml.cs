using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Agents.Net.LogViewer.WpfView;
using Agents.Net.LogViewer.WpfView.Messages;
using Autofac;
using Serilog;
using Serilog.Formatting.Compact;

namespace Agents.Net.LogViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer container;

        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            container?.Dispose();
            base.OnExit(e);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            IMessageBoard messageBoard;

            ConfigureLogging();

            //Create container
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new LogViewerModule());
            container = builder.Build();

            //Start agent community
            try
            {
                messageBoard = container.Resolve<IMessageBoard>();
                Agent[] agents = container.Resolve<IEnumerable<Agent>>().ToArray();
                messageBoard.Register(agents);
                messageBoard.Start();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unhandled exception during setup.");
                return;
            }

            //Show main window
            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

            //Declare MainWindow as Message
            messageBoard.Publish(new MainWindowCreated(mainWindow));
        }

        private void ConfigureLogging()
        {
            File.Delete("log.json");
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.Async(l => l.File(new CompactJsonFormatter(), "log.json"))
                         .CreateLogger();
        }
    }
}