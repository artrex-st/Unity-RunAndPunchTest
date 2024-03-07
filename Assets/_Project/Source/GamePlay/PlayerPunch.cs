using InputSystem;
using UnityEngine;

namespace GamePlay
{
    public class PlayerPunch : MonoBehaviour
    {
        private InputManager _playerInput;
        private PlayerStatus _playerStatus;
        private Transform _punchPosition;

        public void Initialize(InputManager playerInput, PlayerStatus playerStatus ,Transform punchPosition)
        {
            _playerInput = playerInput;
            _playerStatus = playerStatus;
            _punchPosition = punchPosition;
        }
    }
}
