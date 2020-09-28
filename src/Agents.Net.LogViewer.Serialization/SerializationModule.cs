using Autofac;

namespace Agents.Net.LogViewer.Serialization
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Serialization.Agents.LogModelReader>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<Serialization.Agents.LogFileOpener>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<Serialization.Agents.LogEntryModelParser>().As<Agent>().InstancePerLifetimeScope();
        }
    }
}