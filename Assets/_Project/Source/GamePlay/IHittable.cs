using GamePlay;
using UnityEngine;

public interface IHittable
{
    public bool IsKnockDown { get; }
    public void OnHit(PlayerPunch puncher, Vector3 direction);
}
