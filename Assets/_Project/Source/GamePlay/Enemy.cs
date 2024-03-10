using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Utility;

namespace GamePlay
{
    public readonly struct RequestBackpackBodyEvent : IEvent
    {
        public readonly Transform BodyRootBone;

        public RequestBackpackBodyEvent(Transform bodyRootBone)
        {
            BodyRootBone = bodyRootBone;
        }
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IHittable
    {
        public bool IsKnockDown { get; private set; }

        [SerializeField] private Transform _ragdollRoot;
        [SerializeField] private float _hitForce = 50f;
        [SerializeField] private float _hitUpForce = 1f;
        [SerializeField] private float _secondsToBackPack = 1f;

        private Rigidbody RigidBody => GetComponent<Rigidbody>();
        private Animator Animator => GetComponent<Animator>();
        private Collider Collider => GetComponent<Collider>();
        private Rigidbody[] RigidBodies => _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        private CharacterJoint[] Joints => _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        private Collider[] Colliders => _ragdollRoot.GetComponentsInChildren<Collider>();

        private void OnEnable()
        {
            SetRagdoll(false, Vector3.zero);
        }

        public void OnHit(PlayerPunch player, Vector3 direction)
        {
            SetRagdoll(true, direction);
            //TODO: go to BackPack
            StartBagPackEvent();
        }

        private void SetRagdoll(bool enable, Vector3 direction)
        {
            IsKnockDown = enable;
            RigidBody.isKinematic = enable;
            Animator.enabled = !enable;
            Collider.enabled = !enable;

            foreach (CharacterJoint ragJoint in Joints)
            {
                ragJoint.enableCollision = enable;
            }

            foreach (Collider ragCollider in Colliders)
            {
                ragCollider.enabled = enable;
            }

            foreach (Rigidbody ragRigidBody in RigidBodies)
            {
                ragRigidBody.isKinematic = !enable;
            }

            if (direction != Vector3.zero)
            {
                direction = (transform.position - direction).normalized * _hitForce;
                direction += Vector3.up * _hitUpForce;
                RigidBodies[0].velocity = direction;
            }
        }

        private async void StartBagPackEvent()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_secondsToBackPack));
            SyncParentAndRootBone(true);
            new RequestBackpackBodyEvent(transform).Invoke();
        }

        private void SyncParentAndRootBone(bool isEnable)
        {
            RigidBodies[0].isKinematic = RigidBody.isKinematic = isEnable;
            RigidBodies[0].transform.position = transform.position;
            //RigidBodies[0].transform.rotation = transform.rotation;
        }
    }
}
