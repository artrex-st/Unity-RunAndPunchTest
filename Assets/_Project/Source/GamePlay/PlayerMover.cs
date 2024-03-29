using InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay
{
    public class PlayerMover : MonoBehaviour
    {
        private PlayerStatus _status;
        private float _inputDirection;
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
            new RequestBackpackBodyEvent().AddListener(HandlerRequestBackpackBodyEvent);
            new RequestCommitBackpackEvent().AddListener(HandlerRequestCommitBackpackEvent);
        }

        public void Dispose()
        {
            new InputMoveEvent().RemoveListener(HandlerStartInputRotationEvent);
            new RequestBackpackBodyEvent().RemoveListener(HandlerRequestBackpackBodyEvent);
            new RequestCommitBackpackEvent().RemoveListener(HandlerRequestCommitBackpackEvent);
        }

        private void Update()
        {
            ApplyRotation();
            CalculateBodyPile();
        }

        private void FixedUpdate()
        {
            ApplyMove();
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

            for (int i = 0; i < _bodyPile.Count; i++)
            {
                Vector3 target;

                if (i==0)
                {
                    target = _carryPoint.position + Vector3.up;
                }
                else
                {
                    //when distance between body and X,Z of carryPoint is big, the Y target growing down
                    Vector3 position = _carryPoint.position;
                    float distanceArc = Vector2.Distance(
                                                         new Vector2(_bodyPile[i-1].position.x, _bodyPile[i-1].position.z),
                                                         new Vector2(position.x, position.z));
                    float arc = Mathf.Clamp(1 - distanceArc, 0, 1);
                    target = _bodyPile[i-1].position + new Vector3(0,arc,0);
                }

                _bodyPile[i].position = Vector3.Lerp(_bodyPile[i].position, target, _status.PileCarrySpeed * Time.deltaTime);
            }
        }

        private void ApplyMove()
        {
            Vector3 move = transform.forward * (_inputDirection * _status.MoveSpeed);
            Vector3 newPosition = transform.position + move * Time.deltaTime;

            if (!Physics.Raycast(transform.position, transform.forward, move.magnitude * Time.deltaTime, _status.MapLayer))
            {
                _rigidBody.MovePosition(newPosition);
            }
        }

        private void HandlerStartInputRotationEvent(InputMoveEvent e)
        {
            _inputRotation = e.MoveDirection.x;
            _inputDirection = e.MoveDirection.normalized.y;
            new RequestMoveAnimationEvent(_inputDirection).Invoke();
        }

        private void HandlerRequestBackpackBodyEvent(RequestBackpackBodyEvent e)
        {
            Vector3 target;

            if (_bodyPile.Count == 0)
            {
                target = _carryPoint.position + Vector3.up;
            }
            else
            {
                target = _bodyPile[^1].position + Vector3.up;
            }

            e.BodyRootBone.position = target;
            _bodyPile.Add(e.BodyRootBone);
        }

        private void HandlerRequestCommitBackpackEvent(RequestCommitBackpackEvent e)
        {
            //TODO: objectPool?
            new ResponseCommitBackpackEvent(_bodyPile.Count).Invoke();
            foreach (Transform enemyTransform in _bodyPile)
            {
                enemyTransform.GameObject().SetActive(false);
            }

            _bodyPile.Clear();
        }
    }
}
