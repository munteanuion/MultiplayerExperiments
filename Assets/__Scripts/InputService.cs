using System;

namespace DefaultNamespace
{
    public class InputService : IDisposable
    {
        private readonly InputSystemGlobal _inputSystemGlobal;
        private readonly InputSystemGlobal.PlayerActions _playerActions;
        private InputSystemGlobal.IPlayerActions _playerCallbacks;

        public InputService()
        {
            _inputSystemGlobal = new InputSystemGlobal();
            _playerActions = _inputSystemGlobal.Player;
        }

        public void SetPlayerCallbacks(InputSystemGlobal.IPlayerActions playerCallbacks)
        {
            if (_playerCallbacks != null)
            {
                _playerActions.RemoveCallbacks(_playerCallbacks);
            }

            _playerCallbacks = playerCallbacks;

            if (_playerCallbacks != null)
            {
                _playerActions.AddCallbacks(_playerCallbacks);
            }
        }

        public void ClearPlayerCallbacks()
        {
            if (_playerCallbacks == null)
            {
                return;
            }

            _playerActions.RemoveCallbacks(_playerCallbacks);
            _playerCallbacks = null;
        }

        public void EnablePlayerActions()
        {
            _playerActions.Enable();
        }

        public void DisablePlayerActions()
        {
            _playerActions.Disable();
        }

        public void Dispose()
        {
            DisablePlayerActions();
            ClearPlayerCallbacks();
            _inputSystemGlobal.Dispose();
        }
    }
}
