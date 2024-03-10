using UnityEngine;

namespace GamePlay
{
    public readonly struct RequestCommitBackpackEvent : IEvent { }

    [RequireComponent(typeof(Collider))]
    public class GoalManager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                new RequestCommitBackpackEvent().Invoke();
            }
        }
    }
}
