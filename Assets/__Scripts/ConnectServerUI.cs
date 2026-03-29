using System;
using __Scripts._Services.NetworkManagerService;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace DefaultNamespace
{
    public class ConnectServerUI : MonoBehaviour
    {
        [FormerlySerializedAs("_createServerBtn")] 
        [SerializeField] private Button createServerBtn;
        [FormerlySerializedAs("_connectBtn")] 
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
        }

        private void OnDisable()
        {
            createServerBtn.onClick.RemoveListener(OnCreateServerClicked);
            connectBtn.onClick.RemoveListener(OnConnectClicked);
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
        
        private void OnConnectionApprove()
        {
            if (!_networkManager.IsListening)
                _networkManager.StartClient();
            else                
                Debug.LogWarning("Already connected to a server.");
        }
    }
}