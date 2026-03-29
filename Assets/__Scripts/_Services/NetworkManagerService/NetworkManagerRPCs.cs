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

        public bool TrySpawnForServer()
        {
            if (_networkManager == null) return false;
            if (!_networkManager.IsListening || !_networkManager.IsServer) return false;
            if (NetworkObject == null) return false;
            if (IsSpawned) return true;

            NetworkObject.Spawn();
            return IsSpawned;
        }

        public bool TrySendKickMessage(ulong targetClientId, string reason)
        {
            if (_networkManager == null) return false;
            if (!_networkManager.IsServer) return false;
            if (!IsSpawned) return false;

            KickMessageRpc(targetClientId, reason, RpcTarget.Single(targetClientId, RpcTargetUse.Temp));
            return true;
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void KickMessageRpc(ulong targetClientId, string reason, RpcParams _ = default)
        {
            if (_networkManager == null) return;
            if (_networkManager.LocalClientId != targetClientId) return;

            Debug.Log($"Kicked: {reason}");
        }
    }
}