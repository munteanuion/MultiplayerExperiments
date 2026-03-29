using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace __Scripts._Services.NetworkManagerService
{
    public interface INetworkManagerService
    {
        bool IsListening { get; }
        bool IsServer { get; }
        ulong LocalClientId { get; }
        
        NetworkClient LocalClient { get; }
        NetworkConfig NetworkConfig { get; }
        IReadOnlyDictionary<ulong, NetworkClient> ConnectedClients { get; }

        
        void StartHost();
        void StartClient();
        void Shutdown();
        
        
        void DisconnectClient(ulong clientId);
        void Kick(ulong clientId, string reason);
        
        
        void SubscribeToClientConnected(Action<ulong> callback);
        void UnsubscribeFromClientConnected(Action<ulong> callback);
        
        
        void SubscribeToConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback);
        void UnsubscribeFromConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback);
    }
}