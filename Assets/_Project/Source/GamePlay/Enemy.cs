using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IHittable
    {
        public bool IsKnockDown { get; private set; }

        [SerializeField] private Transform _ragdollRoot;
        [SerializeField] private float _hitForce = 50f;
        [SerializeField] private float _hitUpForce = 1f;

        private Animator Animator => GetComponent<Animator>();
        private Rigidbody RigidBody => GetComponent<Rigidbody>();
        private Collider Collider => GetComponent<Collider>();
        private Rigidbody[] RigidBodies => _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        private CharacterJoint[] Joints => _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        private Collider[] Colliders => _ragdollRoot.GetComponentsInChildren<Collider>();


        private void Start()
        {
            SetRagdoll(false, Vector3.zero);
        }

        public void OnHit(PlayerPunch puncher, Vector3 direction)
        {
            //TODO: go to BackPack, disable primary rigidbody and colision
            Debug.Log($"{puncher.gameObject.name} Hit Me ({gameObject.name})");
            SetRagdoll(true, direction);
        }

        private void SetRagdoll(bool enable, Vector3 direction)
        {
            IsKnockDown = enable;
            Animator.enabled = !enable;
            Collider.enabled = !enable;

            if (direction != Vector3.zero)
            {
                direction = (transform.position - direction).normalized * _hitForce;
                direction += Vector3.up * _hitUpForce;
            }

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
                ragRigidBody.velocity = direction;
            }
        }
    }
}
