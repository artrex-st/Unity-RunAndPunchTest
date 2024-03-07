using InputSystem;
using UnityEngine;

namespace GamePlay
{
    public class PlayerPunch : MonoBehaviour
    {
        private InputManager _playerInput;
        public void Initialize(InputManager playerInput)
        {
            _playerInput = playerInput;
        }
    }
}
