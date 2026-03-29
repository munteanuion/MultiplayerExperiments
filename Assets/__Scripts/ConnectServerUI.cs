using __Scripts._Services.NetworkManagerService;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DefaultNamespace
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
        }

        private void OnDisable()
        {
            createServerBtn.onClick.RemoveListener(OnCreateServerClicked);
            connectBtn.onClick.RemoveListener(OnConnectClicked);
            _networkManager.UnsubscribeFromConnectionApproval(OnConnectionApproveCheck);
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
                _networkManager.StartClient();
            else                
                Debug.LogWarning("Already connected to a server.");
        }
        
        private void OnConnectionApproveCheck(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            // exemplu: verifici ceva (parola, capacity, etc.)
            bool approve = !(_networkManager.ConnectedClients.Count >= 4);

            connectionApprovalResponse.Approved = approve;
            connectionApprovalResponse.CreatePlayerObject = approve;
            connectionApprovalResponse.Pending = false;

            connectionApprovalResponse.Reason = approve ? "" : "Server full";
        }
    }
}