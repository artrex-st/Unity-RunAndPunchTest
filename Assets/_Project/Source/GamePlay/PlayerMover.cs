using InputSystem;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GamePlay
{
    public class PlayerMover : MonoBehaviour
    {
        private PlayerStatus _status;
        private float _inputSpeed;
        private float _inputRotation;
        private Rigidbody _rigidBody;
        private Transform _carryPoint;
        private readonly List<Transform> _bodyPile = new List<Transform>();

        public void Initialize(PlayerStatus status, Rigidbody rigidBody, Transform carryPoint)
        {
            _status = status;
            _rigidBody = rigidBody;
            _carryPoint = carryPoint;
            new InputMoveEvent().AddListener(HandlerStartInputRotationEvent);
            new RequestQueueCarryBodyEvent().AddListener(HandlerRequestQueueCarryBodyEvent);
        }

        public void Dispose()
        {
            new InputMoveEvent().RemoveListener(HandlerStartInputRotationEvent);
            new RequestQueueCarryBodyEvent().RemoveListener(HandlerRequestQueueCarryBodyEvent);
        }

        private void Update()
        {
            ApplyRotation();
            CalculateBodyPile();
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

        private void CalculateBodyPile()
        {
            if (_bodyPile.Count < 0)
            {
                return;
            }

            this.LogEditorOnly($"Carry {_bodyPile.Count}");

            for (int i = 0; i < _bodyPile.Count; i++)
            {
                Vector3 target;

                if (i==0)
                {
                    target = _carryPoint.position + Vector3.up;
                }
                else
                {
                    target = _bodyPile[i-1].position + Vector3.up;
                }

                _bodyPile[i].position = Vector3.Lerp(_bodyPile[i].position, target, _status.PileCarrySpeed * Time.deltaTime);
            }
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

        private void HandlerRequestQueueCarryBodyEvent(RequestQueueCarryBodyEvent e)
        {
            Vector3 target;
            if (_bodyPile.Count==0)
            {
                target = _carryPoint.position + Vector3.up;
            }
            else
            {
                target = _bodyPile[^1].position + Vector3.up;
            }
            e.BodyRootBone.position = _carryPoint.position + Vector3.up;
            _bodyPile.Add(e.BodyRootBone);
        }
    }
}
