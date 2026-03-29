using __Scripts._Services.InputService;
using __Scripts._Services.NetworkManagerService;
using VContainer;
using VContainer.Unity;

namespace __Scripts._DIContainer_Bootstrappers
{
    public class GameInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameBootstrapper>();
            
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<NetworkManagerService>(Lifetime.Singleton).As<INetworkManagerService>();
        }
    }
}