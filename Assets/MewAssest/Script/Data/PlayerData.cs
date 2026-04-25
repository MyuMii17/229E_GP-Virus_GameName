using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/BaseData/PlayerData")]

public class PlayerData : BaseData
{
    [Header("Player Settings")]
    [SerializeField] private int maxHP;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxDashCount;
    [SerializeField] private float dashAcceleration;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private float dashCooldownTime;

    public int MaxHP => maxHP;
    public int Damage => damage;
    public float MoveSpeed => moveSpeed;
    public int MaxDashCount => maxDashCount;
    public float DashAcceleration => dashAcceleration;
    public float JumpAcceleration => jumpAcceleration;
    public float AttackCooldownTime => attackCooldownTime;
    public float DashCooldownTime => dashCooldownTime;
}
