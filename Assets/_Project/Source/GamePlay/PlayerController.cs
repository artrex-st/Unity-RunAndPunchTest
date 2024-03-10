using InputSystem;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GamePlay
{
    public readonly struct RequestCanCarryEvent : IEvent { }

    public readonly struct ResponseCanCarryEvent : IEvent
    {
        public readonly bool CanCarry;

        public ResponseCanCarryEvent(bool canCarry)
        {
            CanCarry = canCarry;
        }
    }

    public readonly struct ResponseCommitBackpackEvent : IEvent
    {
        public readonly int BackpackCount;

        public ResponseCommitBackpackEvent(int backpackCount)
        {
            BackpackCount = backpackCount;
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _status;
        [SerializeField] private Transform _punchPoint;
        [SerializeField] private Transform _carryPoint;
        [SerializeField] private SkinnedMeshRenderer _playerSkin;
        [SerializeField] private List<Material> _playerMaterials;

        private PlayerMover _playerMover;
        private PlayerPunch _playerPunch;
        private InputManager _inputManager;
        private bool _isGameRunning;
        private int _currentLevel = 1;
        private int _currentCarry = 0;
        private int _bodyCommitCount = 0;

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
            new RequestCanCarryEvent().AddListener(HandlerRequestCanCarryEvent);
            new RequestBackpackBodyEvent().AddListener(HandlerRequestBackpackBodyEvent);
            new ResponseCommitBackpackEvent().AddListener(HandlerResponseCommitBackpackEvent);
            new RequestLevelUpEvent().AddListener(HandlerRequestLevelUpEvent);

            _inputManager = gameObject.AddComponent<InputManager>();

            _playerMover = gameObject.AddComponent<PlayerMover>();
            _playerMover.Initialize(_status, RigidBody, _carryPoint);
            _playerMover.enabled = false;

            _playerPunch = gameObject.AddComponent<PlayerPunch>();
            _playerPunch.Initialize(_inputManager, _status, _punchPoint);
            _playerPunch.enabled = false;

            _playerSkin.materials[0] = _playerMaterials[_currentLevel];
        }

        private void Dispose()
        {
            new ResponseGameStateUpdateEvent().RemoveListener(HandlerRequestNewGameStateEvent);
            new RequestCanCarryEvent().RemoveListener(HandlerRequestCanCarryEvent);
            new RequestBackpackBodyEvent().RemoveListener(HandlerRequestBackpackBodyEvent);
            new ResponseCommitBackpackEvent().RemoveListener(HandlerResponseCommitBackpackEvent);
            new RequestLevelUpEvent().AddListener(HandlerRequestLevelUpEvent);

            _playerMover.Dispose();
            _playerPunch.Dispose();
        }

        private void HandlerRequestNewGameStateEvent(ResponseGameStateUpdateEvent e)
        {
            _isGameRunning = e.CurrentGameState.Equals(GameStates.GameRunning);
            RigidBody.isKinematic = !_isGameRunning;
            _playerPunch.enabled = _isGameRunning;
            _playerMover.enabled = _isGameRunning;
            new ResponseBuyLevelUpEvent(_currentLevel * _status.MaxCarry).Invoke();
            new RequestMoneyUiEvent(_bodyCommitCount, _currentLevel).Invoke();
        }

        private void HandlerRequestCanCarryEvent(RequestCanCarryEvent e)
        {
            new ResponseCanCarryEvent(_currentCarry < _status.MaxCarry * _currentLevel).Invoke();
        }

        private void HandlerRequestBackpackBodyEvent(RequestBackpackBodyEvent e)
        {
            _currentCarry++;
            new RequestCanCarryEvent().Invoke();
        }

        private void HandlerResponseCommitBackpackEvent(ResponseCommitBackpackEvent e)
        {
            _currentCarry = 0;
            _bodyCommitCount += e.BackpackCount;
            new RequestCanCarryEvent().Invoke();
            new RequestMoneyUiEvent(_bodyCommitCount, _currentLevel).Invoke();
        }

        private void HandlerRequestLevelUpEvent(RequestLevelUpEvent e)
        {
            while (_bodyCommitCount >= _currentLevel * _status.MaxCarry && _currentLevel <= _playerMaterials.Count)
            {
                _bodyCommitCount -= _currentLevel * _status.MaxCarry;
                _currentLevel++;
                new ResponseBuyLevelUpEvent(_currentLevel * _status.MaxCarry).Invoke();
                new RequestCanCarryEvent().Invoke();
                new RequestMoneyUiEvent(_bodyCommitCount, _currentLevel).Invoke();
                _playerSkin.material = _playerMaterials[_currentLevel];
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_punchPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_punchPoint.position, _status.PunchRadius);
            }
        }
#endif
    }
}
