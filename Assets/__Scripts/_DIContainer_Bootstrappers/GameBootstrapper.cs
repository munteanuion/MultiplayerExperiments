using System;
using __Scripts._Services.InputService;
using __Scripts._Services.NetworkManagerService;
using Unity.Netcode;
using VContainer.Unity;

namespace __Scripts._DIContainer_Bootstrappers
{
    public class GameBootstrapper : IStartable, ITickable, IDisposable
    {
        private readonly IInputService _inputService;
        private bool _isInputEnabled;
        private INetworkManagerService _networkManagerService;

        
        public GameBootstrapper(IInputService inputService, INetworkManagerService networkManagerService)
        {
            _networkManagerService = networkManagerService;
            _inputService = inputService;
        }

        
        public void Start()
        {
            _inputService.DisablePlayerActions();
            _isInputEnabled = false;
            _networkManagerService.Init();
        }

        public void Tick()
        {
            bool shouldEnableInput = HasSpawnedLocalOwnerPlayer();
            if (shouldEnableInput == _isInputEnabled)
            {
                return;
            }

            if (shouldEnableInput)
            {
                _inputService.EnablePlayerActions();
            }
            else
            {
                _inputService.DisablePlayerActions();
            }

            _isInputEnabled = shouldEnableInput;
        }

        private bool HasSpawnedLocalOwnerPlayer()
        {
            INetworkManagerService networkManager = _networkManagerService;
            if (networkManager == null)
            {
                return false;
            }

            if (!networkManager.IsListening)
            {
                return false;
            }

            if (networkManager.LocalClient == null)
            {
                return false;
            }

            NetworkObject localPlayerObject = networkManager.LocalClient.PlayerObject;
            if (localPlayerObject == null)
            {
                return false;
            }

            if (!localPlayerObject.IsSpawned)
            {
                return false;
            }

            return localPlayerObject.IsOwner;
        }

        public void Dispose()
        {
            _inputService.DisablePlayerActions();
            _isInputEnabled = false;
            _inputService.Dispose();
        }
    }
}