using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class PlayerInputController : MonoBehaviour
    {
        private StarterAssetsInputs _input;
        private void Awake()
        {
            _input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            // Handle input, can be extended as needed (e.g., using Input System).
            // For example, setting movement and jump inputs.
            if (_input != null)
            {
                // Example: You could add custom logic for other inputs.
            }
        }

        public Vector2 GetMovementInput()
        {
            return _input.move;
        }

        public bool IsSprinting()
        {
            return _input.sprint;
        }

        public bool IsJumping()
        {
            return _input.jump;
        }
    }
}