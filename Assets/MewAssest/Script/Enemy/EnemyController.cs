using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private DataHolder dataHolder;
    private HashSet<SpriteRenderer>enemySpriteRenderer = new HashSet<SpriteRenderer>();
    private FindPlayer findPlayer;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Coroutine attackCoroutine;
    private Animator enemyAnimator;
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
    public bool isFindPlayer;
    public bool isClash;
    public bool isAttacked;
    void Awake()
    {
        dataHolder = GetComponent<DataHolder>();
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        findPlayer = transform.GetChild(1).gameObject.GetComponent<FindPlayer>();
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
        for(int i = 2 ; i < 10; i++)
        {
            SpriteRenderer sprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            enemySpriteRenderer.Add(sprite);
        }
    }

    public IEnumerator OnHit(float damage, float pushForce)
    {
        isAttacked = true;

        enemyHP -= damage;

        if(playerController.currentHealRequirment < playerController.playerHealRequirement)
        {
            playerController.currentHealRequirment++;
        }
        else if(playerController.currentHealRequirment > playerController.playerHealRequirement)
        {
            playerController.currentHealRequirment = playerController.playerHealRequirement;
        }

        var dir = playerController.transform.position - transform.position ;
        dir.Normalize();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-dir * pushForce, ForceMode2D.Impulse);
        if(enemyHP <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
        yield return new WaitForSeconds(1f);
        isAttacked = false;
    }
    public void ClashToPlayer(float pushForce)
    {
        StartCoroutine(Clash(pushForce));
    }
    IEnumerator Clash(float pushForce)
    {
        var dir = playerController.transform.position - transform.position ;
        dir.Normalize();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-dir * pushForce * 1.5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        isClash = false;
    }

    public void OnFindPlayer()
    {
        var dir = transform.position - playerController.transform.position;
        dir.Normalize();
        rb.linearVelocity = new Vector2(-dir.x * enemyMoveSpeed, rb.linearVelocity.y);

        if (dir.x >= 0)
        {
            foreach(var spriteRenderer in enemySpriteRenderer)
            {
                spriteRenderer.flipX = true;
            }
        }
        else if (dir.x <= 0)
        {
            foreach(var spriteRenderer in enemySpriteRenderer)
            {
                spriteRenderer.flipX = false;
            }
        }

    }
    void Update()
    {
        if (findPlayer.playerInRange.Contains(playerController))
        {
            isFindPlayer = true;
        }
        else
        {
            isFindPlayer = false;
        }

        if(isHasHit == true)
        {
            isHasHit = false;
            if(attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(OnHit(playerController.playerDamage,playerController.playerPushForce));
        }
        if(rb.linearVelocity != Vector2.zero && isClash == false && isAttacked == false)
        {
            enemyAnimator.SetBool("isRunning",true);
        }
        else if(rb.linearVelocity == Vector2.zero)
        {
            enemyAnimator.SetBool("isRunning",false);
        }
    }
    void FixedUpdate()
    {
        if(isFindPlayer == true && isClash == false && isAttacked == false)
        {
            OnFindPlayer();
        }
    }
}
