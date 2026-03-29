using __Scripts._Services.NetworkManagerService;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace __Scripts
{
    public class ConnectServerUI : MonoBehaviour
    {
        [SerializeField] private Button createServerBtn;
        [SerializeField] private Button connectBtn;
        
        private INetworkManagerService _networkManager;

        [Inject]
        private void Construct(INetworkManagerService networkManager)
        {
            _networkManager = networkManager;
        }
        

        private void OnEnable()
        {
            createServerBtn.onClick.AddListener(OnCreateServerClicked);
            connectBtn.onClick.AddListener(OnConnectClicked);
            _networkManager.SubscribeToConnectionApproval(OnConnectionApproveCheck);
                _networkManager.SubscribeToClientConnected(OnClientConnected);
        }

        private void OnDisable()
        {
            createServerBtn.onClick.RemoveListener(OnCreateServerClicked);
            connectBtn.onClick.RemoveListener(OnConnectClicked);
            _networkManager.UnsubscribeFromConnectionApproval(OnConnectionApproveCheck);
            _networkManager.UnsubscribeFromClientConnected(OnClientConnected);
            _networkManager.Shutdown();
        }

        private void OnCreateServerClicked()
        {
            if (!_networkManager.IsListening)
                _networkManager.StartHost();
            else
                Debug.LogWarning("Already connected to a server.");
        }

        private void OnConnectClicked()
        {
            if (!_networkManager.IsListening)
            {
                var payload = System.Text.Encoding.UTF8.GetBytes("parola123");
                _networkManager.NetworkConfig.ConnectionData = payload;
                _networkManager.StartClient();
            }
            else                
                Debug.LogWarning("Already connected to a server.");
            
        }
        
        private void OnConnectionApproveCheck(
            NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, 
            NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            bool approve = false;
            var request = connectionApprovalRequest;
            var response = connectionApprovalResponse;
            
            var data = request.Payload;
            var text = System.Text.Encoding.UTF8.GetString(data);
            
            approve = 
                !(_networkManager.ConnectedClients.Count >= 4)
                && text == "parola123"
                || _networkManager.IsServer;

            response.Approved = approve;
            response.CreatePlayerObject = approve;
            response.Pending = false;

            response.Reason = approve ? "Connection Success" : "Server full";
        }
        
        
        private void OnClientConnected(ulong clientId)
        {
            if (!_networkManager.IsServer) return;

            if (_networkManager.ConnectedClients.Count >= 4)
            {
                _networkManager.Kick(clientId, "Server full");
                return;
            }
        }
        
        
        [ContextMenu("Kick Last Client")]
        private void KickLastClient()
        {
            if (!_networkManager.IsServer) return;
            
            var clientCount = _networkManager.ConnectedClients.Count;
            
            foreach (var client in _networkManager.ConnectedClients)
            {
                if (client.Key == _networkManager.LocalClientId && _networkManager.IsServer) continue;
                _networkManager.Kick(client.Key, "Random kick");
            }
        }
    }
}