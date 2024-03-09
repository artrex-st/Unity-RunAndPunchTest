using InputSystem;
using System;
using UnityEngine;

namespace GamePlay
{
    public class PlayerPunch : MonoBehaviour
    {
        private InputManager _playerInput;
        private PlayerStatus _playerStatus;
        private Transform _punchPosition;
        private bool _canPunch = true;

        public void Initialize(InputManager playerInput, PlayerStatus playerStatus ,Transform punchPosition)
        {
            _playerInput = playerInput;
            _playerStatus = playerStatus;
            _punchPosition = punchPosition;
            new RequestInputPressEvent().AddListener(HandlerRequestInputPressEvent);
            new ResponsePunchAnimationEvent().AddListener(HandlerResponsePunchAnimationEvent);
        }

        public void Dispose()
        {
            new RequestInputPressEvent().RemoveListener(HandlerRequestInputPressEvent);
            new ResponsePunchAnimationEvent().RemoveListener(HandlerResponsePunchAnimationEvent);
        }

        private void HandlerRequestInputPressEvent(RequestInputPressEvent e)
        {
            if (!_canPunch)
            {
                return;
            }

            _canPunch = false;

            Collider[] results = new Collider[_playerStatus.MaxHitPerAttack];
            int size = Physics.OverlapSphereNonAlloc(_punchPosition.position, _playerStatus.PunchRadius, results, _playerStatus.HittableLayer);

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (results[i].TryGetComponent(out IHittable hittable))
                    {
                        hittable.OnHit(this, transform.position);
                    }
                }
            }
        }

        private void HandlerResponsePunchAnimationEvent(ResponsePunchAnimationEvent e)
        {
            if (e.InstanceId == transform.GetInstanceID())
            {
                _canPunch = true;
            }
        }
    }
}
