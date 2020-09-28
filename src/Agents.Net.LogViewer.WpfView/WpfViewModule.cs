using Autofac;

namespace Agents.Net.LogViewer.WpfView
{
    public class WpfViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WpfView.Agents.ViewModelSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<WpfView.Agents.GraphViewModelSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<WpfView.Agents.DetailViewModelSynchronizer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<WpfView.Agents.SelectionModifier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<WpfView.Agents.MainWindowObserver>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
        }
    }
}