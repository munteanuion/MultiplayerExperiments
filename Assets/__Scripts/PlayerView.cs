using __Scripts._Services.InputService;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace __Scripts
{
    public class PlayerView : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 6f;

        private IInputService _inputService;

        
        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        
        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (characterController == null)
            {
                return;
            }

            if (_inputService == null)
            {
                return;
            }

            Vector2 moveInput = _inputService.MoveInput;
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            characterController.SimpleMove(moveDirection * moveSpeed);
        }
    }
}