using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace __Scripts._Services.InputService
{
    public class InputService : IInputService
    {
        private const string PlayerActionMapName = "Player";
        private const string MoveActionName = "Move";
        private const string LookActionName = "Look";
        private const string JumpActionName = "Jump";
        private const string SprintActionName = "Sprint";

        private readonly InputSystemGlobal _inputSystemGlobal;
        private readonly InputActionMap _playerActionMap;
        private readonly InputAction _moveAction;
        private readonly InputAction _lookAction;
        private readonly InputAction _jumpAction;
        private readonly InputAction _sprintAction;
        private readonly Action<InputAction.CallbackContext> _movePerformedHandler;
        private readonly Action<InputAction.CallbackContext> _moveCanceledHandler;
        private readonly Action<InputAction.CallbackContext> _lookPerformedHandler;
        private readonly Action<InputAction.CallbackContext> _lookCanceledHandler;
        private readonly Action<InputAction.CallbackContext> _jumpPerformedHandler;
        private readonly Action<InputAction.CallbackContext> _sprintPerformedHandler;
        private readonly Action<InputAction.CallbackContext> _sprintCanceledHandler;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _jumpPressed;
        private bool _isSprintHeld;

        public InputService()
        {
            _inputSystemGlobal = new InputSystemGlobal();
            _playerActionMap = _inputSystemGlobal.asset.FindActionMap(PlayerActionMapName, true);
            _moveAction = _playerActionMap.FindAction(MoveActionName, true);
            _lookAction = _playerActionMap.FindAction(LookActionName, true);
            _jumpAction = _playerActionMap.FindAction(JumpActionName, true);
            _sprintAction = _playerActionMap.FindAction(SprintActionName, true);

            _movePerformedHandler = _ => _moveInput = _moveAction.ReadValue<Vector2>();
            _moveCanceledHandler = _ => _moveInput = Vector2.zero;
            _lookPerformedHandler = _ => _lookInput = _lookAction.ReadValue<Vector2>();
            _lookCanceledHandler = _ => _lookInput = Vector2.zero;
            _jumpPerformedHandler = _ => _jumpPressed = true;
            _sprintPerformedHandler = _ => _isSprintHeld = true;
            _sprintCanceledHandler = _ => _isSprintHeld = false;

            _moveAction.performed += _movePerformedHandler;
            _moveAction.canceled += _moveCanceledHandler;
            _lookAction.performed += _lookPerformedHandler;
            _lookAction.canceled += _lookCanceledHandler;
            _jumpAction.performed += _jumpPerformedHandler;
            _sprintAction.performed += _sprintPerformedHandler;
            _sprintAction.canceled += _sprintCanceledHandler;
        }

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;
        public bool IsSprintHeld => _isSprintHeld;

        public void EnablePlayerActions()
        {
            _playerActionMap.Enable();
        }

        public void DisablePlayerActions()
        {
            _playerActionMap.Disable();
            _moveInput = Vector2.zero;
            _lookInput = Vector2.zero;
            _jumpPressed = false;
            _isSprintHeld = false;
        }

        public bool ConsumeJumpPressed()
        {
            if (!_jumpPressed)
            {
                return false;
            }

            _jumpPressed = false;
            return true;
        }

        public void Dispose()
        {
            _moveAction.performed -= _movePerformedHandler;
            _moveAction.canceled -= _moveCanceledHandler;
            _lookAction.performed -= _lookPerformedHandler;
            _lookAction.canceled -= _lookCanceledHandler;
            _jumpAction.performed -= _jumpPerformedHandler;
            _sprintAction.performed -= _sprintPerformedHandler;
            _sprintAction.canceled -= _sprintCanceledHandler;

            DisablePlayerActions();
            _inputSystemGlobal.Dispose();
        }
    }
}
