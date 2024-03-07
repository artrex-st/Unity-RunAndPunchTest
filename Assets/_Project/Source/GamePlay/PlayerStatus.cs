using UnityEngine;

[CreateAssetMenu(menuName = "Services/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private float _punchRadius = 5;

    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float PunchRadius => _punchRadius;
}

