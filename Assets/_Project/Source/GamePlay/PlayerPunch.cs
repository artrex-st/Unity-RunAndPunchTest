using InputSystem;
using UnityEngine;

namespace GamePlay
{
    public readonly struct RequestPunchEvent : IEvent { }
    public readonly struct ResponsePunchAnimationEvent : IEvent
    {
        public readonly int InstanceId;

        public ResponsePunchAnimationEvent(int speed)
        {
            InstanceId = speed;
        }
    }

    public class PlayerPunch : MonoBehaviour
    {
        private InputManager _playerInput;
        private PlayerStatus _playerStatus;
        private Transform _punchPosition;
        private bool _punchAnimationReady = true;
        private bool _canCarry = true;

        public void Initialize(InputManager playerInput, PlayerStatus playerStatus ,Transform punchPosition)
        {
            _playerInput = playerInput;
            _playerStatus = playerStatus;
            _punchPosition = punchPosition;
            new RequestInputPressEvent().AddListener(HandlerRequestInputPressEvent);
            new ResponsePunchAnimationEvent().AddListener(HandlerResponsePunchAnimationEvent);
            new ResponseCanCarryEvent().AddListener(HandlerResponseCanCarryEvent);
        }

        public void Dispose()
        {
            new RequestInputPressEvent().RemoveListener(HandlerRequestInputPressEvent);
            new ResponsePunchAnimationEvent().RemoveListener(HandlerResponsePunchAnimationEvent);
            new ResponseCanCarryEvent().RemoveListener(HandlerResponseCanCarryEvent);
        }

        private void HandlerRequestInputPressEvent(RequestInputPressEvent e)
        {
            if (!_punchAnimationReady || !_canCarry)
            {
                return;
            }

            new RequestPunchEvent().Invoke();
            _punchAnimationReady = false;

            Collider[] results = new Collider[_playerStatus.MaxHitPerAttack];
            int size = Physics.OverlapSphereNonAlloc(_punchPosition.position, _playerStatus.PunchRadius, results, _playerStatus.HittableLayer);

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (results[i].TryGetComponent(out IHittable hittable))
                    {
                        hittable.OnHit(this, transform.position);
                        _canCarry = false;
                    }
                }
            }
        }

        private void HandlerResponsePunchAnimationEvent(ResponsePunchAnimationEvent e)
        {
            if (e.InstanceId == transform.GetInstanceID())
            {
                _punchAnimationReady = true;
            }
        }

        private void HandlerResponseCanCarryEvent(ResponseCanCarryEvent e)
        {
            _canCarry = e.CanCarry;
        }
    }
}
