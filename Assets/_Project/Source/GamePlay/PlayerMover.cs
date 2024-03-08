using InputSystem;
using UnityEngine;

namespace GamePlay
{
    public class PlayerMover : MonoBehaviour
    {
        private PlayerStatus _status;
        private float _inputSpeed;
        private float _inputRotation;
        private Rigidbody _rigidBody;

        public void Initialize(PlayerStatus status, Rigidbody rigidBody)
        {
            _status = status;
            _rigidBody = rigidBody;
            new InputMoveEvent().AddListener(HandlerStartInputRotationEvent);
        }

        public void Dispose()
        {
            new InputMoveEvent().RemoveListener(HandlerStartInputRotationEvent);
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
            Vector3 speed = transform.forward * (_inputSpeed * _status.MoveSpeed);
            speed.y = _rigidBody.velocity.y;
            _rigidBody.velocity = speed;
        }

        private void HandlerStartInputRotationEvent(InputMoveEvent e)
        {
            _inputRotation = e.MoveRotation;
            _inputSpeed = e.MoveDirection;
            new RequestMoveAnimationEvent(_inputSpeed).Invoke();
        }
    }
}
