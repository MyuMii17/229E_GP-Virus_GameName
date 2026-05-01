using UnityEngine;

public enum EnemyType{ Swordman, Archer }

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/BaseData/EnemyData")]
public class EnemyData : BaseData
{
    [Header("Enemy Setting")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float enemyHP;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushAcceleration;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private GameObject arrowPrefeb;
    public string EnemyType => enemyType.ToString();
    public float EnemyHP => enemyHP;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float PushAcceleration => pushAcceleration;
    public float AttackCooldownTime => attackCooldownTime;
    public GameObject ArrowPrefeb => arrowPrefeb;

}
