using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace __Scripts._Services.NetworkManagerService
{
    public class NetworkManagerService : INetworkManagerService
    {
        private NetworkManager NetworkManager;
        
        public bool IsListening => NetworkManager != null && NetworkManager.IsListening;
        public NetworkClient LocalClient => NetworkManager != null ? NetworkManager.LocalClient : null;
        public IReadOnlyDictionary<ulong,NetworkClient> ConnectedClients => NetworkManager != null ? NetworkManager.ConnectedClients : null;


        public NetworkManagerService(NetworkManager networkManager)
        {
            NetworkManager = networkManager;
        }

        
        
        public void StartHost()
        {
            NetworkManager.StartHost();
        }

        public void StartClient()
        {
            NetworkManager.StartClient();
        }
        
        
        
        public void SubscribeToConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback)
        {
            if (NetworkManager != null)
                NetworkManager.ConnectionApprovalCallback += callback;
        }
        public void UnsubscribeFromConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback)
        {
            if (NetworkManager != null)
                NetworkManager.ConnectionApprovalCallback -= callback;
        }
    }
}