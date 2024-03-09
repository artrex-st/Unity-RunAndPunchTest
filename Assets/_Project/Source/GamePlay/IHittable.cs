using GamePlay;
using UnityEngine;

public interface IHittable
{
    public bool IsKnockDown { get; }
    public void OnHit(PlayerPunch player, Vector3 direction);
}
