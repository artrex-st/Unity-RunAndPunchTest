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

        public void Initialize(InputManager playerInput, PlayerStatus playerStatus ,Transform punchPosition)
        {
            _playerInput = playerInput;
            _playerStatus = playerStatus;
            _punchPosition = punchPosition;
            new RequestInputPressEvent().AddListener(HandlerRequestInputPressEvent);
        }

        public void Dispose()
        {
            new RequestInputPressEvent().RemoveListener(HandlerRequestInputPressEvent);
        }

        private void HandlerRequestInputPressEvent(RequestInputPressEvent e)
        {
            Debug.Log($"Press Input! {e.CurrentPhase}");
            Vector3 center = transform.position;

            Collider[] results = new Collider[3];
            int size = Physics.OverlapSphereNonAlloc(center, _playerStatus.PunchRadius, results, _playerStatus.HittableLayer);

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (results[i].TryGetComponent(out IHittable hittable))
                    {
                        hittable.OnHit(this);
                    }
                    Debug.Log($"Hit: {results[i].gameObject.name}");
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_punchPosition.position, _playerStatus.PunchRadius+1f);
        }
    }
}
