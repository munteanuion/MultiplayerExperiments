using Unity.Netcode;

namespace __Scripts._Services.NetworkManagerService
{
    public class NetworkManagerService : INetworkManagerService
    {
        private NetworkManager NetworkManager => NetworkManager.Singleton;
        
        public bool IsListening => NetworkManager != null && NetworkManager.IsListening;
        public NetworkClient LocalClient => NetworkManager != null ? NetworkManager.LocalClient : null;

        public void StartHost()
        {
            NetworkManager.StartHost();
        }

        public void StartClient()
        {
            NetworkManager.StartClient();
        }
    }
}