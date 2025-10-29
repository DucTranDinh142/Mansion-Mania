using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnimator { get; private set; }
    public Rigidbody2D playerRigidbody { get; private set; }
    public PlayerInputSet input { get; private set; }


    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }

    [Header("Attack Stats")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration;
    public float comboResetTime;
    private Coroutine queuedAttackCoroutine;

    [Header("Movement Stats")]
    public float moveSpeed;
    public float jumpForce;

    public Vector2 wallJumpDirection { get; private set; }
    [Range(0f, 1f)]
    public float moveInAirSpeedMultiplier;
    [Range(0f, 1f)]
    public float wallSlideSpeedMultiplier;
    [Space]
    public float dashDuration;
    public float dashSpeed;
    public Vector2 moveInput { get; private set; }

    private bool facingRight = true;
    public int facingDirectionValue { get; private set; } = 1;

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform[] wallCheckTransform;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(stateMachine, "Idle", this);
        moveState = new Player_MoveState(stateMachine, "Move", this);
        jumpState = new Player_JumpState(stateMachine, "JumpFall", this);
        fallState = new Player_FallState(stateMachine, "JumpFall", this);
        wallSlideState = new Player_WallSlideState(stateMachine, "WallSlide", this);
        wallJumpState = new Player_WallJumpState(stateMachine, "JumpFall", this);
        dashState = new Player_DashState(stateMachine, "Dash", this);
        basicAttackState = new Player_BasicAttackState(stateMachine, "BasicAttack", this);
        jumpAttackState = new Player_JumpAttackState(stateMachine, "JumpAttack", this);
    }
    private void OnEnable()
    {
        input.Enable();
        //input.Player.Movement.started
        input.Player.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        input.Player.Movement.canceled += context => moveInput = Vector2.zero;
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }
    private void Update()
    {
        HandleCollisionDetection();
        wallJumpDirection = new Vector2(moveSpeed * moveInAirSpeedMultiplier, jumpForce);
        stateMachine.UpdateActiveState();
    }
    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);
        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCoroutine());
    }
    private IEnumerator EnterAttackStateWithDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        playerRigidbody.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity < 0 && facingRight == true)
            Flip();
        else if (xVelocity > 0 && facingRight != true)
            Flip();
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirectionValue *= -1;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheckTransform[0].position, Vector2.right * facingDirectionValue, wallCheckDistance, whatIsGround)
            && Physics2D.Raycast(wallCheckTransform[1].position, Vector2.right * facingDirectionValue, wallCheckDistance, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckDistance));
        Gizmos.DrawLine(wallCheckTransform[0].position, wallCheckTransform[0].position + new Vector3(wallCheckDistance * facingDirectionValue, 0));
        Gizmos.DrawLine(wallCheckTransform[1].position, wallCheckTransform[1].position + new Vector3(wallCheckDistance * facingDirectionValue, 0));
    }
}
