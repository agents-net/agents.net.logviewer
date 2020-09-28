using Autofac;
using Agents.Net;
using Agents.Net.LogViewer.Model;
using Agents.Net.LogViewer.Serialization;
using Agents.Net.LogViewer.ViewModel;
using Agents.Net.LogViewer.WpfView;

namespace Agents.Net.LogViewer
{
    public class LogViewerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageBoard>().As<IMessageBoard>().InstancePerLifetimeScope();
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<SerializationModule>();
            builder.RegisterModule<ViewModelModule>();
            builder.RegisterModule<WpfViewModule>();
        }
    }
}
