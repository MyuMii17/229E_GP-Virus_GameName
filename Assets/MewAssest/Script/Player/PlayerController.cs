using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    [SerializeField]private Transform attackAreaPos;
    [SerializeField]private GameObject attackAreaHit;
    private Animator playerAnimator;
    private HashSet<SpriteRenderer> playerRenderer = new HashSet<SpriteRenderer>();
    private AttackAreaList attackAreaList;
    private AttackArea attackArea;
    private SpriteRenderer playerSprite;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction blockAction;
    private InputAction healAction;
    private GameObject slashVfxSpawn;
    private Coroutine healCoroutine;
    private float playerMoveSpeed;
    private float playerMass;
    private float playerDashCount;
    private float playerMaxDashCount;
    private float playerDashAcceleration;
    private float playerJumpAcceleration;
    private float playerPushAcceleration;
    private float playerDashCooldown;
    private float playerJumpForce;
    private float playerDashForce;
    private float playerGravityScale;
    private float playerLinearDamp;
    private float playerAngularDamp;
    private float playerDamageReduction;
    private float playerMaxHP;
    private float horizontalInput;
    private float verticalInput;

    [Header("Player Setting")]
    public Rigidbody2D rb;
    public float playerCurrentHP;
    public float playerDamage;
    public float playerPushForce;
    public float playerAttackCooldown;
    public int currentHealRequirment;
    public int playerHealRequirement;
    public bool isDashing;
    public bool isDashCooldown;
    public bool isMove;
    public bool isJumpPressed;
    public bool isGrounded;
    public bool isAttacking;
    public bool isImmune;
    public bool isHasHit;
    public bool isGameOver;
    public bool isBlocking;
    public bool isBlockingVfxSpawn;
    public bool isCanHeal;
    public bool isHealing;

    private static PlayerController StaticInstance = null;
    public static PlayerController GetStatic()
    {
        return StaticInstance;
    }

    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        StaticInstance = this;

        isDashing = false;
        isImmune = false;
        isHasHit = false;

        currentHealRequirment = 0;

        dataHolder = GetComponent<DataHolder>();
        rb = GetComponent<Rigidbody2D>();

        attackAreaPos = transform.GetChild(0).transform;
        attackAreaHit = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        playerSprite = GetComponent<SpriteRenderer>();

        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        dashAction = InputSystem.actions.FindAction("Dash");
        blockAction = InputSystem.actions.FindAction("Block");
        healAction = InputSystem.actions.FindAction("Heal");

        if(dataHolder.baseData is PlayerData playerData)
        {
            playerMass = dataHolder.baseData.Mass;
            playerJumpAcceleration = playerData.JumpAcceleration;
            playerDashAcceleration = playerData.DashAcceleration;
            playerLinearDamp = dataHolder.baseData.LinearDamp;
            playerAngularDamp = dataHolder.baseData.AngularDamp;
            playerMoveSpeed = playerData.MoveSpeed;
            playerGravityScale = dataHolder.baseData.GravityScale;
            playerMaxDashCount = playerData.MaxDashCount;
            playerDashCooldown = playerData.DashCooldownTime;
            playerAttackCooldown = playerData.AttackCooldownTime;
            playerDamage = playerData.Damage;
            playerMaxHP = playerData.MaxHP;
            playerPushAcceleration = playerData.PushAcceleration;
            playerDamageReduction = playerData.DamageReduction;
            playerHealRequirement = playerData.HealRequirement;

            rb.mass = playerMass;
            rb.linearDamping = playerLinearDamp;
            rb.angularDamping = playerAngularDamp;
            rb.gravityScale = playerGravityScale;
            playerDashCount = playerMaxDashCount;
            playerCurrentHP = playerMaxHP;

            playerJumpForce = rb.mass * playerJumpAcceleration;
            playerDashForce = rb.mass * playerDashAcceleration;
            playerPushForce = rb.mass * playerPushAcceleration;

        }
        else
        {
            Debug.LogError("BaseData is not of type PlayerData.");
        }

    }
    void Start()
    {
        attackAreaList = AttackAreaList.GetStatic();
        attackArea = AttackArea.GetStatic();
        playerAnimator = GetComponent<Animator>();

        for(int i = 1 ; i < 9; i++)
        {
            SpriteRenderer sprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            playerRenderer.Add(sprite);
        }

    }

    void Update()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        verticalInput = moveAction.ReadValue<Vector2>().y;

        if(horizontalInput != 0 && moveAction.WasPressedThisFrame())
            {
                playerAnimator.SetBool("isWalk",true);
            }
            else if(horizontalInput == 0 && !moveAction.WasPressedThisFrame())
            {
                playerAnimator.SetBool("isWalk",false);
            }

        if (horizontalInput < 0 && isAttacking == false && isDashing == false && isHasHit == false && isBlocking == false) 
        { 
            foreach(var playerSprite in playerRenderer)
            {
                playerSprite.flipX = true;
            }
            attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (horizontalInput > 0  && isAttacking == false && isDashing == false && isHasHit == false && isBlocking == false) 
        {
            foreach(var playerSprite in playerRenderer)
            {
                playerSprite.flipX = false;
            }
            attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if(verticalInput > 0 && isDashing == false && isGrounded && isBlocking == false)
        {
            isJumpPressed = true;
        }

        if (dashAction.WasPressedThisFrame() && isHasHit == false && isDashing == false && isDashCooldown == false && horizontalInput != 0 && playerDashCount > 0 && isBlocking == false)
        {
            StartCoroutine(Dash(horizontalInput));
        }   

        if(attackAction.WasPressedThisFrame() && isAttacking != true && isHasHit == false && isBlocking == false)
        {
            StartCoroutine(Attack());
        }

        if (blockAction.IsPressed() && isAttacking != true && isHasHit == false)
        {
            //blockAni
            isBlocking = true;
        }
        else
        {
            //blockAni
            isBlocking = false;
        }

        if (healAction.WasPressedThisFrame() && isBlocking == false && isAttacking == false && isHasHit == false && isDashing == false )
        {
            if(currentHealRequirment >= playerHealRequirement)
            {
                if(healCoroutine != null)
                {
                    StopCoroutine(healCoroutine);
                }
                healCoroutine = StartCoroutine(Heal());
            }
        }
    }

    void FixedUpdate()
    {

        if(horizontalInput != 0 && isDashing == false && isHasHit == false && isBlocking == false)
        {
            rb.linearVelocity = new Vector2( horizontalInput * playerMoveSpeed, rb.linearVelocity.y);
        }

        if(isJumpPressed == true && isHasHit == false && isBlocking == false)
        {
            rb.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            isJumpPressed = false;
        }

    }
    IEnumerator Dash(float dir)
    {
        playerAnimator.SetBool("isDash",true);
        var direction = dir; 
        isDashing = true;
        isDashCooldown = true;
        isImmune = true;
        playerDashCount--;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(direction * playerDashForce, 0);

        yield return new WaitForSeconds(0.5f);

        rb.gravityScale = playerGravityScale;
        playerAnimator.SetBool("isDash",false);
        isDashing = false;
        isImmune = false;

        yield return new WaitForSeconds(playerDashCooldown);
        isDashCooldown = false;
    }
    IEnumerator Attack()
    {
        playerAnimator.SetBool("isAttack",true);
        isAttacking = true;
        attackAreaHit.SetActive(true);
        attackArea.OnAttack();
        yield return new WaitForSeconds(playerAttackCooldown);
        playerAnimator.SetBool("isAttack",false);
        attackAreaHit.SetActive(false);
        isAttacking = false;
    }
    public IEnumerator OnHit(float damage, Vector2 dir, float pushForce)
    {
        if(isImmune == true) yield break;
        playerAnimator.SetBool("isTakeDamage",true);
        if(isBlocking == true)
        {
            damage *= playerDamageReduction;
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-dir * pushForce , ForceMode2D.Impulse);
        playerCurrentHP -= damage;

        if(playerCurrentHP <= 0)
        {
            isGameOver = true;
            Time.timeScale = 0;
        }
        yield return new WaitForSeconds(0.4f);
        playerAnimator.SetBool("isTakeDamage",false);
        isHasHit = false;
    }
    IEnumerator Heal()
    {
        playerAnimator.SetBool("isHeal",true);
        playerCurrentHP = playerMaxHP;
        currentHealRequirment = 0;
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool("isHeal",false);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        playerDashCount = playerMaxDashCount;
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
