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
    [SerializeField]private Transform wallCheck;
    [SerializeField]private LayerMask wallLayer;
    private Animator playerAnimator;
    private HeartManager heartManager;
    private HealBarManager healBarManager;
    private HashSet<SpriteRenderer> playerRenderer = new HashSet<SpriteRenderer>();
    private AttackAreaList attackAreaList;
    private AttackArea attackArea;
    private SpriteRenderer playerSprite;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction blockAction;
    private InputAction healAction;

    private Coroutine healCoroutine;
    private float playerMoveSpeed;
    private float playerMass;
    public float playerDashAcceleration;
    private float playerJumpAcceleration;
    private float playerPushAcceleration;
    private float playerDashCooldown;
    private float playerJumpForce;
    private float playerDashForce;
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
    public float currentHealRequirment;
    public float playerHealRequirement;
    public float playerGravityScale;

    [Header("What Player Can Do?")]
    public bool isCanMove;
    public bool isCanJump;
    public bool isCanDash;
    public bool isCanBlock;
    public bool isCanAttack;
    public bool isCanHeal;

    [Header("What Player Doing?")]
    public bool isMoveing;
    public bool isJumpPressed;
    public bool isJumping;
    public bool isDashing;
    public bool isImmune;
    public bool isAttacking;
    public bool isHealing;
    public bool isBlocking;
    public bool isHasHit;
    public bool isHitEnemy;
    public bool isGrounded;
    public bool isWalled;
    public bool isGameOver;

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

        isCanMove = true;
        isCanAttack = true;
        isCanBlock = true;
        isCanDash = true;
        isCanJump = true;

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
        healBarManager = HealBarManager.GetStatic();
        heartManager = HeartManager.GetStatic();
        playerAnimator = GetComponent<Animator>();

        for(int i = 1 ; i < 9; i++)
        {
            SpriteRenderer sprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            playerRenderer.Add(sprite);
        }

    }

    void Update()
    {
        healBarManager.SetHealReqBar(currentHealRequirment,playerHealRequirement);
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        verticalInput = moveAction.ReadValue<Vector2>().y;

        // If Moveing Player Change =>  Idel to Walk, FlipX, Rotation AttaclArea
        if(horizontalInput != 0 && isCanMove)
        {
            isMoveing = true;
            playerAnimator.SetBool("isWalk",true);

            if (horizontalInput < 0 && isAttacking == false && isDashing == false && isHasHit == false && isBlocking == false && isHealing == false) 
            { 
                foreach(var playerSprite in playerRenderer)
                {
                    playerSprite.flipX = true;
                }
                attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (horizontalInput > 0  && isAttacking == false && isDashing == false && isHasHit == false && isBlocking == false && isHealing == false) 
            {
                foreach(var playerSprite in playerRenderer)
                {
                    playerSprite.flipX = false;
                }
                attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if(horizontalInput == 0)
        {
            isMoveing = false;
            playerAnimator.SetBool("isWalk",false);
        }

        // If Jumping Player Change => isJumpPressed, 
        if(verticalInput > 0 && isDashing == false && isBlocking == false && isHealing == false && isHasHit == false && isAttacking == false && isGrounded && isCanJump)
        {
            isJumpPressed = true;
        }
        else if(verticalInput > 0 && isDashing == false && isBlocking == false && isHealing == false && isHasHit == false && isAttacking == false && isWalled)
        {
            isJumpPressed = true;
        }
        else if(verticalInput == 0)
        {
            isJumpPressed = false;
        }


        if (dashAction.WasPressedThisFrame() && horizontalInput != 0 && isHasHit == false && isBlocking == false  && isHealing == false && isAttacking == false && isCanDash)
        {
            isCanDash = false;
            StartCoroutine(Dash(horizontalInput));
        }   
        

        if(attackAction.WasPressedThisFrame() && isHasHit == false && isBlocking == false && isHealing == false && isDashing == false && isCanAttack)
        {
            isCanAttack = false;
            StartCoroutine(Attack());
        }


        if (blockAction.IsPressed() && isAttacking == false && isHasHit == false && isHealing == false && isDashing == false && isJumping == false && isCanBlock)
        {
            isBlocking = true;

            foreach(var playerSprite in playerRenderer)
            {
                playerSprite.color = Color.yellow;
            }
        }
        else
        {
            isBlocking = false;
        }
        

        if(currentHealRequirment >= playerHealRequirement && playerCurrentHP < playerMaxHP)
        {
            isCanHeal = true;
        }


        if (healAction.WasPressedThisFrame() && isBlocking == false && isAttacking == false && isHasHit == false && isDashing == false && isMoveing == false && isJumping == false && isCanHeal)
        {
            isCanHeal = false;

            foreach(var playerSprite in playerRenderer)
            {
                playerSprite.color = Color.green;
            }

            if(healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
            }

            healCoroutine = StartCoroutine(Heal());

        }


        if(isBlocking == false && isHasHit == false && isHealing == false)
        {
            foreach(var playerSprite in playerRenderer)
            {
                playerSprite.color = Color.white;
            }
        }

    }

    void FixedUpdate()
    {

        if(isMoveing)
        {
            rb.linearVelocity = new Vector2( horizontalInput * playerMoveSpeed, rb.linearVelocity.y);
        }


        if(isJumpPressed && isCanJump)
        {
            isCanJump = false;
            StartCoroutine(Jump());
        }

    }


    IEnumerator Jump()
    {
        if(isCanJump == true) yield break;

        rb.linearVelocity = Vector2.zero;

        if(isGrounded)
        {
            if (isWalled)
            {
                rb.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            }
        }


        if(isWalled && !isGrounded)
        {

            rb.linearVelocity = new Vector2(-horizontalInput * 10, playerJumpForce);

        }

        yield return new WaitForSeconds(0.5f);

        isCanJump = true;

    }


    IEnumerator Dash(float dir)
    {

        if(isCanDash == true) yield break;
        
        playerAnimator.SetBool("isDash",true);
        
        isCanMove = false;
        isDashing = true;
        isImmune = true;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(dir * playerDashForce, 0);

        yield return new WaitForSeconds(0.5f);

        playerAnimator.SetBool("isDash",false);

        rb.gravityScale = playerGravityScale;

        isDashing = false;
        isImmune = false;

        isCanMove = true;

        yield return new WaitForSeconds(playerDashCooldown);

        isCanDash = true;
        
    }

    
    IEnumerator Attack()
    {

        if(isCanAttack == true) yield break;

        playerAnimator.SetBool("isAttack",true);

        isAttacking = true;
        attackAreaHit.SetActive(true);

        attackArea.OnAttack();

        yield return new WaitForSeconds(playerAttackCooldown);

        playerAnimator.SetBool("isAttack",false);

        attackAreaHit.SetActive(false);
        isAttacking = false;
        isCanAttack = true;

    }


    public IEnumerator OnHit(float damage, Vector2 dir, float pushForce)
    {
        if(isImmune == true) yield break;
        playerAnimator.SetBool("isTakeDamage",true);

        foreach(var playerSprite in playerRenderer)
        {
            playerSprite.color = Color.red;
        }

        if(isBlocking == true)
        {
            damage *= playerDamageReduction;
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-dir * pushForce , ForceMode2D.Impulse);
        playerCurrentHP -= damage;
        heartManager.OnDamageSetHeart(damage);

        if(playerCurrentHP <= 0)
        {
            isGameOver = true;
            Time.timeScale = 0;
        }
        yield return new WaitForSeconds(0.4f);
        foreach(var playerSprite in playerRenderer)
        {
            playerSprite.color = Color.white;
        }
        playerAnimator.SetBool("isTakeDamage",false);

        isHasHit = false;
    }
    IEnumerator Heal()
    {
        isHealing = true;
        playerAnimator.SetBool("isHeal",true);
        playerCurrentHP = playerMaxHP;
        heartManager.OnHealSetHeart();
        currentHealRequirment = 0;
        yield return new WaitForSeconds(0.5f);

        isHealing = false;
        playerAnimator.SetBool("isHeal",false);

        isCanHeal = true;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {    
            isGrounded = true;
            isCanMove = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && isGrounded == false)
        {
            isCanMove = false;
            isWalled = true;
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isCanMove = true;
            isWalled = false;
        }
    }

}
