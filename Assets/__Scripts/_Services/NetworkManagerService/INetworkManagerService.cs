using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace __Scripts._Services.NetworkManagerService
{
    public interface INetworkManagerService
    {
        bool IsListening { get; }
        NetworkClient LocalClient { get; }
        IReadOnlyDictionary<ulong, NetworkClient> ConnectedClients { get; }

        void StartHost();
        void StartClient();
        
        void SubscribeToConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback);
        void UnsubscribeFromConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback);
    }
}