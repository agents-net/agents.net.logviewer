using Autofac;

namespace Agents.Net.LogViewer.ViewModel
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewModel.Agents.AgentsViewModelReader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.AgentsViewModelAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.MessageViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.MessagesViewModelReader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.DetailViewModelWatcher>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.LogViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.MicrosoftGraph.Agents.GraphMapInformationGatherer>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.MessagesViewModelAggregator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.ViewModelCrossReferencing>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.MicrosoftGraph.Agents.GraphCache>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.Agents.AgentViewModelCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.MicrosoftGraph.Agents.GraphCreator>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModel.MicrosoftGraph.Agents.GraphToViewModelTranslator>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}