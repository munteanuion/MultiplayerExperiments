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

            var networkManagerRPCs = InitializeNetworkManagerRPCs();

            builder.RegisterComponent(networkManagerRPCs);
            builder.RegisterComponent(networkManager);

            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<NetworkManagerService>(Lifetime.Singleton).As<INetworkManagerService>();
        }

        private NetworkManagerRPCs InitializeNetworkManagerRPCs()
        {
            var networkManagerRpcObject = new GameObject(nameof(NetworkManagerRPCs));
            DontDestroyOnLoad(networkManagerRpcObject);

            networkManagerRpcObject.AddComponent<NetworkObject>();

            var networkManagerRPCs = networkManagerRpcObject.AddComponent<NetworkManagerRPCs>();
            networkManagerRPCs.Init(networkManager);
            return networkManagerRPCs;
        }
    }
}
