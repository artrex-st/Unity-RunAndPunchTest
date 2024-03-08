using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IHittable
    {
        public bool IsKnockDown { get; private set; }

        [SerializeField] private Transform _ragdollRoot;

        private Animator Animator => GetComponent<Animator>();
        private Rigidbody RigidBody => GetComponent<Rigidbody>();
        private Collider Collider => GetComponent<Collider>();
        private Rigidbody[] RigidBodies => _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        private CharacterJoint[] Joints => _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        private Collider[] Colliders => _ragdollRoot.GetComponentsInChildren<Collider>();


        private void Start()
        {
            SetRagdoll(false);
        }

        public void OnHit(PlayerPunch puncher)
        {
            //TODO: go to BackPack, disable primary rigidbody and colision
            Debug.Log($"{puncher.gameObject.name} Hit Me ({gameObject.name})");
            SetRagdoll(true);
        }

        private void SetRagdoll(bool enable)
        {
            IsKnockDown = enable;
            Animator.enabled = !enable;
            Collider.enabled = !enable;

            foreach (CharacterJoint ragJoint in Joints)
            {
                ragJoint. enableCollision = enable;
            }

            foreach (Collider ragCollider in Colliders)
            {
                ragCollider.enabled = enable;
            }

            foreach (Rigidbody ragRigidBody in RigidBodies)
            {
                ragRigidBody.detectCollisions = enable;
                ragRigidBody.useGravity = enable;
                ragRigidBody.velocity = Vector3.zero;
            }
        }
    }
}
