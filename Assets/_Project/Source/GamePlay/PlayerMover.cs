using InputSystem;
using UnityEngine;

namespace GamePlay
{
    public class PlayerMover : MonoBehaviour
    {
        private PlayerStatus _status;
        private float _inputForward;
        private float _inputRotation;
        private Rigidbody _rigidBody;

        public void Initialize(PlayerStatus status, Rigidbody rigidBody)
        {
            _status = status;
            _rigidBody = rigidBody;
            new InputXEvent().AddListener(HandlerStartInputRotationEvent);
            new InputYEvent().AddListener(HandlerStartInputForwardEvent);
        }

        public void Dispose()
        {
            new InputXEvent().RemoveListener(HandlerStartInputRotationEvent);
            new InputYEvent().RemoveListener(HandlerStartInputForwardEvent);
        }

        private void Update()
        {
            ApplyRotation();
        }

        private void FixedUpdate()
        {
            ApplySpeed();
        }

        private void ApplyRotation()
        {
            float rotationAmount = _inputRotation * _status.RotationSpeed * Time.deltaTime;
            Quaternion inputRotation = Quaternion.Euler(0f, rotationAmount, 0f);
            transform.rotation *= inputRotation;
        }

        private void ApplySpeed()
        {
            Vector3 speed = transform.forward * (_inputForward * _status.MoveSpeed);
            speed.y = _rigidBody.velocity.y;
            _rigidBody.velocity = speed;
        }

        private void HandlerStartInputForwardEvent(InputYEvent e)
        {
            _inputForward = e.MoveForward;
            new RequestMoveAnimationEvent(_inputForward).Invoke();
        }

        private void HandlerStartInputRotationEvent(InputXEvent e)
        {
            _inputRotation = e.MoveRotation;
        }
    }
}
