using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/BaseData/PlayerData")]

public class PlayerData : BaseData
{
    [Header("Player Settings")]
    [SerializeField] private float maxHP;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxDashCount;
    [SerializeField] private float pushAcceleration;
    [SerializeField] private float dashAcceleration;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private float dashCooldownTime;
    [SerializeField] private float damageReduction;
    [SerializeField] private int healRequirement;

    public float MaxHP => maxHP;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public int MaxDashCount => maxDashCount;
    public float PushAcceleration => pushAcceleration;
    public float DashAcceleration => dashAcceleration;
    public float JumpAcceleration => jumpAcceleration;
    public float AttackCooldownTime => attackCooldownTime;
    public float DashCooldownTime => dashCooldownTime;
    public float DamageReduction => damageReduction;
    public int HealRequirement => healRequirement;
}
