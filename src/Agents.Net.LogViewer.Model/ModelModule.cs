using Autofac;

namespace Agents.Net.LogViewer.Model
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Model.Agents.LogModelAggregator>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}