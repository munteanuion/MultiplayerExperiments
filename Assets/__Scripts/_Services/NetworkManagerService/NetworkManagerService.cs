using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace __Scripts._Services.NetworkManagerService
{
    public class NetworkManagerService : INetworkManagerService
    {
        private const float KickDisconnectDelaySeconds = 2f;

        private readonly NetworkManager _networkManager;
        private readonly NetworkManagerRPCs _networkManagerRPCs;


        public bool IsListening => _networkManager != null && _networkManager.IsListening;
        public bool IsServer => _networkManager != null && _networkManager.IsServer;
        public ulong LocalClientId => _networkManager.LocalClientId;
        
        
        public NetworkClient LocalClient => _networkManager != null ? _networkManager.LocalClient : null;
        public NetworkConfig NetworkConfig => _networkManager != null ? _networkManager.NetworkConfig : null;
        public IReadOnlyDictionary<ulong,NetworkClient> ConnectedClients => _networkManager != null ? _networkManager.ConnectedClients : null;


        
        public NetworkManagerService(
            NetworkManager networkManager,
            NetworkManagerRPCs networkManagerRPCs)
        {
            _networkManagerRPCs = networkManagerRPCs;
            _networkManager = networkManager;
        }

        
        
        public void StartHost()
        {
            var isStarted = _networkManager.StartHost();
            if (!isStarted) return;

            _networkManagerRPCs.TrySpawnForServer();
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
            if (_networkManager == null) return;
            if (!_networkManager.IsListening || !_networkManager.IsServer) return;
            if (clientId == _networkManager.LocalClientId) return;
            if (!_networkManager.ConnectedClients.ContainsKey(clientId)) return;
            if (!_networkManagerRPCs.TrySpawnForServer())
            {
                _networkManager.DisconnectClient(clientId);
                return;
            }

            Debug.Log($"Kick client {clientId}: {reason}");

            _networkManagerRPCs.TrySendKickMessage(clientId, reason);
            _networkManager.DisconnectClient(clientId);
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