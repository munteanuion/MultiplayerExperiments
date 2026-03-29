using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerView : MonoBehaviour, InputSystemGlobal.IPlayerActions
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 6f;

        private InputService _inputService;
        private Vector2 _moveInput;

        private void Awake()
        {
            _inputService = new InputService();
            _inputService.SetPlayerCallbacks(this);
        }

        private void OnEnable()
        {
            _inputService.EnablePlayerActions();
        }

        private void Update()
        {
            if (characterController == null)
            {
                return;
            }

            var moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
            characterController.SimpleMove(moveDirection * moveSpeed);
        }

        private void OnDisable()
        {
            if (_inputService == null)
            {
                return;
            }

            _inputService.DisablePlayerActions();
        }

        private void OnDestroy()
        {
            if (_inputService == null)
            {
                return;
            }

            _inputService.ClearPlayerCallbacks();
            _inputService.Dispose();
            _inputService = null;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }
    }
}