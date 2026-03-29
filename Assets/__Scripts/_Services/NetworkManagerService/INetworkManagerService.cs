using Unity.Netcode;

namespace __Scripts._Services.NetworkManagerService
{
    public interface INetworkManagerService
    {
        bool IsListening { get; }
        NetworkClient LocalClient { get; }
        
        void StartHost();
        void StartClient();
    }
}