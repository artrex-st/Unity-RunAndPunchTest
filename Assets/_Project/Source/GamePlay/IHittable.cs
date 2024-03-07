using GamePlay;
using UnityEngine;

public interface IHittable
{
    public Rigidbody GetRigidBody();

    public void OnHit(PlayerPunch puncher);
}
