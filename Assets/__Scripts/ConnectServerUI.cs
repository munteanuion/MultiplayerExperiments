using __Scripts._Services.NetworkManagerService;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace __Scripts
{
    public class ConnectServerUI : MonoBehaviour
    {
        private const string ServerPassword = "parola123";
        private const string InvalidClientPassword = "parola1234";
        private const string AlreadyConnectedMessage = "Already connected to a server.";
        private const string ConnectionSuccessMessage = "Connection Success";
        private const string ConnectionFailedMessage = "Server full or Invalid Password";
        private const string KickServerFullMessage = "Server full";
        private const string KickRandomMessage = "Random kick";
        private const string ConnectionRejectedWithoutReasonMessage = "Connection rejected.";

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
            _networkManager.SubscribeToClientDisconnected(OnClientDisconnected);
        }

        private void OnDisable()
        {
            createServerBtn.onClick.RemoveListener(OnCreateServerClicked);
            connectBtn.onClick.RemoveListener(OnConnectClicked);
            _networkManager.UnsubscribeFromConnectionApproval(OnConnectionApproveCheck);
            _networkManager.UnsubscribeFromClientConnected(OnClientConnected);
            _networkManager.UnsubscribeFromClientDisconnected(OnClientDisconnected);
            _networkManager.Shutdown();
        }

        private void OnCreateServerClicked()
        {
            if (!_networkManager.IsListening)
            {
                _networkManager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(ServerPassword);
                _networkManager.StartHost();
            }
            else
                Debug.LogWarning(AlreadyConnectedMessage);
        }

        private void OnConnectClicked()
        {
            if (!_networkManager.IsListening)
            {
                _networkManager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(InvalidClientPassword);
                _networkManager.StartClient();
            }
            else                
                Debug.LogWarning(AlreadyConnectedMessage);
            
        }
        
        private void OnConnectionApproveCheck(
            NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, 
            NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            var request = connectionApprovalRequest;
            var response = connectionApprovalResponse;
            
            var data = request.Payload;
            var text = Encoding.UTF8.GetString(data);
            
            var approve = 
                !(_networkManager.ConnectedClients.Count >= 4)
                && text == ServerPassword;

            response.Approved = approve;
            response.CreatePlayerObject = approve;
            response.Pending = false;

            response.Reason = approve ? ConnectionSuccessMessage : ConnectionFailedMessage;
        }
        
        
        private void OnClientConnected(ulong clientId)
        {
            if (!_networkManager.IsServer) return;

            if (_networkManager.ConnectedClients.Count >= 4)
            {
                _networkManager.Kick(clientId, KickServerFullMessage);
                return;
            }
        }

        private void OnClientDisconnected(ulong clientId, string reason)
        {
            if (_networkManager.IsServer) return;
            if (clientId != _networkManager.LocalClientId) return;

            var message = string.IsNullOrWhiteSpace(reason) ? ConnectionRejectedWithoutReasonMessage : reason;
            Debug.LogWarning(message);
        }
        
        
        [ContextMenu("Kick Last Client")]
        private void KickLastClient()
        {
            if (!_networkManager.IsServer) return;
            
            foreach (var client in _networkManager.ConnectedClients)
            {
                if (client.Key == _networkManager.LocalClientId && _networkManager.IsServer) continue;
                _networkManager.Kick(client.Key, KickRandomMessage);
            }
        }
    }
}