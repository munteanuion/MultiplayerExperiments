using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace __Scripts._Services.NetworkManagerService
{
    public class NetworkManagerService : INetworkManagerService
    {
        private readonly NetworkManager _networkManager;
        
        
        public bool IsListening => _networkManager != null && _networkManager.IsListening;
        public bool IsServer => _networkManager != null && _networkManager.IsServer;
        public ulong LocalClientId => _networkManager.LocalClientId;
        
        
        public NetworkClient LocalClient => _networkManager != null ? _networkManager.LocalClient : null;
        public NetworkConfig NetworkConfig => _networkManager != null ? _networkManager.NetworkConfig : null;
        public IReadOnlyDictionary<ulong,NetworkClient> ConnectedClients => _networkManager != null ? _networkManager.ConnectedClients : null;


        
        public NetworkManagerService(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        
        
        public void StartHost()
        {
            _networkManager.StartHost();
        }

        public void StartClient()
        {
            _networkManager.StartClient();
        }

        public void Shutdown()
        {
            _networkManager.Shutdown();
        }


        public void DisconnectClient(ulong clientId)
        {
            _networkManager.DisconnectClient(clientId);
        }
        
        public void Kick(ulong clientId, string reason)
        {
            Debug.Log($"Kick client {clientId}: {reason}");

            KickMessageRpc(clientId, reason);
            _networkManager.DisconnectClient(clientId);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void KickMessageRpc(ulong targetClientId, string reason)
        {
            if (_networkManager.LocalClientId != targetClientId) return;

            Debug.Log($"Kicked: {reason}");
        }


        
        public void SubscribeToConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback)
        {
            if (_networkManager != null)
                _networkManager.ConnectionApprovalCallback += callback;
        }
        public void UnsubscribeFromConnectionApproval(Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> callback)
        {
            if (_networkManager != null)
                _networkManager.ConnectionApprovalCallback -= callback;
        }

        
        
        public void SubscribeToClientConnected(Action<ulong> callback)
        {
            if (_networkManager != null)
                _networkManager.OnClientConnectedCallback += callback;
        }
        
        public void UnsubscribeFromClientConnected(Action<ulong> callback)
        {
            if (_networkManager != null)
                _networkManager.OnClientConnectedCallback -= callback;
        }
    }
}