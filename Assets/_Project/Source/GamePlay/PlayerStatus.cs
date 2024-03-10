using UnityEngine;

[CreateAssetMenu(menuName = "Services/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [Header("Move")]
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private LayerMask _mapLayer;
    [Header("Attack")]
    [SerializeField] private float _punchRadius = 5;
    [SerializeField] private LayerMask _hittableLayer;
    [SerializeField][Range(0,10)] private int _maxHitPerAttack = 1;
    [Header("Carry")]
    [SerializeField] private int _maxCarry = 2;
    [SerializeField] private float _pileCarrySpeed = 1;

    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public LayerMask MapLayer => _mapLayer;
    public float PunchRadius => _punchRadius;
    public LayerMask HittableLayer => _hittableLayer;
    public int MaxHitPerAttack => _maxHitPerAttack;
    public int MaxCarry => _maxCarry;
    public float PileCarrySpeed => _pileCarrySpeed;
}

