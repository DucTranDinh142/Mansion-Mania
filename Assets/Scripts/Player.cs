using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator {  get; private set; }
    public Rigidbody2D playerRigidbody {  get; private set; }
    public PlayerInputSet input { get; private set; }


    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }


    [Header("Movement Stats")]
    public float moveSpeed;
    [Range(0f, 1f)]
    public float moveInJumpSpeedMultiplier;
    public float jumpForce;
    public Vector2 moveInput { get; private set; }

    private bool facingRight = true;
    //private float facingrightvalue = 1f;

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(stateMachine,"Idle", this);
        moveState = new Player_MoveState(stateMachine,"Move", this);
        jumpState = new Player_JumpState(stateMachine, "Jump/Fall", this);
        fallState = new Player_FallState(stateMachine, "Jump/Fall", this);
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
        stateMachine.UpdateActiveState();
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
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position,Vector2.down,groundCheckDistance,whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckDistance));
    }
}
