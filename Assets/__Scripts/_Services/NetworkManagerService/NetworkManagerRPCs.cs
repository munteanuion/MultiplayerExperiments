using Unity.Netcode;
using UnityEngine;

namespace __Scripts._Services.NetworkManagerService
{
    public class NetworkManagerRPCs : NetworkBehaviour
    {
        private NetworkManager _networkManager;

        public void Init(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        public void KickMessageRpc(ulong targetClientId, string reason)
        {
            if (_networkManager.LocalClientId != targetClientId) return;

            Debug.Log($"Kicked: {reason}");
        }
    }
}