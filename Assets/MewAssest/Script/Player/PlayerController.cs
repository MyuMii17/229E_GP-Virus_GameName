using System.Collections;
using System.Data.Common;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    [SerializeField]private Transform attackAreaPos;
    [SerializeField]private GameObject attackArea;
    [SerializeField]private GameObject slashVFX;
    [SerializeField]private Transform slashPos;
    private SpriteRenderer playerSprite;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction moveAction;
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
    private float playerPushForce;
    private float playerGravityScale;
    private float playerLinearDamp;
    private float playerAngularDamp;
    private float horizontalInput;
    private float verticalInput;

    [Header("Player Setting")]
    public Rigidbody2D rb;
    public float playerHP;
    public float playerDamage;
    public float playerAttackCooldown;
    public bool isDashing;
    public bool isDashCooldown;
    public bool isMove;
    public bool isJumpPressed;
    public bool isGrounded;
    public bool isAttacking;
    public bool isImmune;
    public bool isHasHit;
    public bool isGameOverl;

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

        dataHolder = GetComponent<DataHolder>();
        rb = GetComponent<Rigidbody2D>();
        attackAreaPos = transform.GetChild(0).transform;
        attackArea = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        playerSprite = GetComponent<SpriteRenderer>();
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        dashAction = InputSystem.actions.FindAction("Dash");

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
            playerHP = playerData.MaxHP;
            playerPushAcceleration = playerData.PushAcceleration;

            rb.mass = playerMass;
            rb.linearDamping = playerLinearDamp;
            rb.angularDamping = playerAngularDamp;
            rb.gravityScale = playerGravityScale;
            playerDashCount = playerMaxDashCount;

            playerJumpForce = rb.mass * playerJumpAcceleration;
            playerDashForce = rb.mass * playerDashAcceleration;
            playerPushForce = rb.mass * playerPushAcceleration;

        }
        else
        {
            Debug.LogError("BaseData is not of type PlayerData.");
        }

    }
    void Update()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        verticalInput = moveAction.ReadValue<Vector2>().y;
        if (horizontalInput < 0 && isAttacking == false && isDashing == false && isHasHit == false) 
        { 
            playerSprite.flipX = true;
            attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (horizontalInput > 0  && isAttacking == false && isDashing == false && isHasHit == false) 
        {
            playerSprite.flipX = false;
            attackAreaPos.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if(verticalInput > 0 && isDashing == false && isGrounded)
        {
            isJumpPressed = true;
        }

        if (dashAction.WasPressedThisFrame() && isHasHit == false && isDashing == false && isDashCooldown == false && horizontalInput != 0 && playerDashCount > 0)
        {
            
            StartCoroutine(Dash(horizontalInput));
        }   
        if(attackAction.WasPressedThisFrame() && isAttacking != true && isHasHit == false)
        {
            var slashVfxSpawn = Instantiate(slashVFX,slashPos.position,Quaternion.identity);
            Destroy(slashVfxSpawn, 0.2f);
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {

        if(horizontalInput != 0 && isDashing == false && isHasHit == false)
        {
            rb.linearVelocity = new Vector2( horizontalInput * playerMoveSpeed, rb.linearVelocity.y);
        }

        if(isJumpPressed == true && isHasHit == false)
        {
            rb.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            isJumpPressed = false;
        }

    }
    IEnumerator Dash(float dir)
    {
        var direction = dir; 
        isDashing = true;
        isDashCooldown = true;
        isImmune = true;
        playerDashCount--;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(direction * playerDashForce, 0);

        yield return new WaitForSeconds(0.5f);

        rb.gravityScale = playerGravityScale;
        isDashing = false;
        isImmune = false;

        yield return new WaitForSeconds(playerDashCooldown);
        isDashCooldown = false;
    }
    IEnumerator Attack()
    {
        isAttacking = true;
        attackArea.SetActive(true);
        AttackArea.GetStatic().OnAttack();
        yield return new WaitForSeconds(playerAttackCooldown);
        attackArea.SetActive(false);
        isAttacking = false;
    }
    public IEnumerator HasHit()
    {
        yield return new WaitForSeconds(0.4f);
        isHasHit = false;
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

    public void OnHit(float damage, Vector2 dir, float pushForce)
    {
        if(isImmune==true) return;
        
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-dir * pushForce , ForceMode2D.Impulse);
        playerHP -= damage;

        if(playerHP <= 0)
        {
            isGameOverl = true;
            Time.timeScale = 0;
        }
    }
}
