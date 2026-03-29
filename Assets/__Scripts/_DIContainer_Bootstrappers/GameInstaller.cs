using __Scripts._Services.InputService;
using __Scripts._Services.NetworkManagerService;
using Unity.Netcode;
using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace __Scripts._DIContainer_Bootstrappers
{
    public class GameInstaller : LifetimeScope
    {
        [SerializeField] private NetworkManager networkManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameBootstrapper>();
            
            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<NetworkManagerService>(Lifetime.Singleton)
                .WithParameter(networkManager)
                .As<INetworkManagerService>();
        }
    }
}