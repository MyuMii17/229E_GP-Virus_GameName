using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private DataHolder dataHolder;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private float enemyMass;
    private float enemyLinerDamp;
    private float enemyAugularDamp;
    private float enemyGravityScale;
    private float enemyMoveSpeed;
    private float enemyPushAcceleration;
    private float enemyAttackCooldownTime;
    private GameObject enemyArrowPrefeb;

    [Header("Enemy Setting")]
    public float enemyHP;
    public float enemyDamage;
    public float enemyPushForce;
    public bool isCanDestroy;
    public bool isHasHit;
    void Awake()
    {
        isHasHit = false;
        dataHolder = GetComponent<DataHolder>();
        rb = GetComponent<Rigidbody2D>();
        if(dataHolder.baseData is EnemyData enemyData)
        {
            enemyHP = enemyData.EnemyHP;
            enemyMass = dataHolder.baseData.Mass;
            enemyLinerDamp = dataHolder.baseData.LinearDamp;
            enemyAugularDamp = dataHolder.baseData.AngularDamp;
            enemyGravityScale = dataHolder.baseData.GravityScale;
            isCanDestroy = dataHolder.baseData.IsCanDestroy;
            enemyDamage = enemyData.Damage;
            enemyMoveSpeed = enemyData.MoveSpeed;
            enemyPushAcceleration = enemyData.PushAcceleration;
            enemyAttackCooldownTime = enemyData.AttackCooldownTime;
            if(enemyData.EnemyType == "Archer")
            {
                enemyArrowPrefeb = enemyData.ArrowPrefeb;
            }

            rb.mass = enemyMass;
            rb.linearDamping = enemyLinerDamp;
            rb.angularDamping = enemyAugularDamp;
            rb.gravityScale = enemyGravityScale;

            enemyPushForce = rb.mass * enemyPushAcceleration;
        }
    }

    void Start()
    {
        playerController = PlayerController.GetStatic();
    }

    public void OnHit(float damage, float pushForce)
    {
        enemyHP -= damage;
        var dir = transform.position - playerController.transform.position;
        dir.Normalize();
        rb.AddForce(dir * pushForce, ForceMode2D.Impulse);
        isHasHit = false;

        if(enemyHP <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
