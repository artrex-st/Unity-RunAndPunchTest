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
            Vector3 center = transform.position;

            Collider[] results = new Collider[_playerStatus.MaxHitPerAttack];
            int size = Physics.OverlapSphereNonAlloc(center, _playerStatus.PunchRadius, results, _playerStatus.HittableLayer);

            if (size != 0)
            {
                Debug.Log($"Hit: {size}");
                for (int i = 0; i < size; i++)
                {
                    Debug.Log($"Hit: {results[i].gameObject.name}");
                    if (results[i].TryGetComponent(out IHittable hittable))
                    {
                        hittable.OnHit(this);
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_punchPosition.position, _playerStatus.PunchRadius+1f);
        }
    }
}
