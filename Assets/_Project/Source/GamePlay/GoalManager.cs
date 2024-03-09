using System;
using UnityEngine;
using Utility;

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
                this.LogEditorOnly($"{playerController.name} Hit GoalTrigger");
                new RequestCommitBackpackEvent().Invoke();
            }
        }
    }
}
