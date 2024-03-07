using InputSystem;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _status;

        private PlayerMover _playerMover;
        private PlayerPunch _playerPunch;
        private InputManager _inputManager;
        private bool _isGameRunning;
        private Rigidbody RigidBody => GetComponent<Rigidbody>();

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            new ResponseGameStateUpdateEvent().AddListener(HandlerRequestNewGameStateEvent);

            _inputManager = gameObject.AddComponent<InputManager>();

            _playerMover = gameObject.AddComponent<PlayerMover>();
            _playerMover.Initialize(_status, RigidBody);

            _playerPunch = gameObject.AddComponent<PlayerPunch>();
            _playerPunch.Initialize(_inputManager);
        }

        private void Dispose()
        {
            new ResponseGameStateUpdateEvent().RemoveListener(HandlerRequestNewGameStateEvent);
            _playerMover.Dispose();
        }

        private void HandlerRequestNewGameStateEvent(ResponseGameStateUpdateEvent e)
        {
            _isGameRunning = e.CurrentGameState.Equals(GameStates.GameRunning);
            RigidBody.isKinematic = !_isGameRunning;
            _playerPunch.enabled = _isGameRunning;
            _playerMover.enabled = _isGameRunning;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            //Gizmos.DrawRay(start.position, target.position);
        }
#endif
    }
}
