using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ConnectServerUI : MonoBehaviour
    {
        [SerializeField] private Button _createServerBtn;
        [SerializeField] private Button _connectBtn;
        
        private NetworkManager _networkManager => NetworkManager.Singleton;

        private void OnEnable()
        {
            _createServerBtn.onClick.AddListener(OnCreateServerClicked);
            _connectBtn.onClick.AddListener(OnConnectClicked);
        }

        private void OnDisable()
        {
            _createServerBtn.onClick.RemoveListener(OnCreateServerClicked);
            _connectBtn.onClick.RemoveListener(OnConnectClicked);
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