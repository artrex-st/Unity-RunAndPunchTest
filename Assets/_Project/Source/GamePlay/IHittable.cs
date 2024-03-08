using GamePlay;

public interface IHittable
{
    public bool IsKnockDown { get; }
    public void OnHit(PlayerPunch puncher);
}
