using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    private Rigidbody2D rb;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction moveAction;
    private float playerMoveSpeed;
    private float playerJumpForce;
    private float playerDashForce;
    private float playerMass;
    private float playerGravityScale;
    private float playerDashCount;
    private float playerMaxDashCount;
    private float playerDashAcceleration;
    private float playerJumpAcceleration;
    private float playerDashCooldown;
    private float playerLinearDamp;
    private float playerAngularDamp;
    private float horizontalInput;
    private float verticalInput;
    public bool isDashing;
    public bool isDashCooldown;
    public bool isMove;
    public bool isJumpPressed;
    public bool isGrounded;
    void Start()
    {
        isDashing = false;

        dataHolder = GetComponent<DataHolder>();
        rb = GetComponent<Rigidbody2D>();
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

            rb.mass = playerMass;
            rb.linearDamping = playerLinearDamp;
            rb.angularDamping = playerAngularDamp;
            rb.gravityScale = playerGravityScale;
            playerDashCount = playerMaxDashCount;

            playerJumpForce = rb.mass * playerJumpAcceleration;
            playerDashForce = rb.mass * playerDashAcceleration;

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

        if(verticalInput > 0 && isDashing == false && isGrounded)
        {
            isJumpPressed = true;
        }

        if (dashAction.WasPressedThisFrame() && isDashing == false && isDashCooldown == false && horizontalInput != 0 && playerDashCount > 0)
        {
            
            StartCoroutine(Dash(horizontalInput));
        }   
    }

    void FixedUpdate()
    {

        if(horizontalInput != 0 && isDashing == false)
        {
            rb.linearVelocity = new Vector2( horizontalInput * playerMoveSpeed, rb.linearVelocity.y);
        }

        if(isJumpPressed == true)
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
        playerDashCount--;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(direction * playerDashForce, 0);

        yield return new WaitForSeconds(0.5f);

        rb.gravityScale = playerGravityScale;
        isDashing = false;

        yield return new WaitForSeconds(playerDashCooldown);
        isDashCooldown = false;
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
