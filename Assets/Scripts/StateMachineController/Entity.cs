using UnityEngine;


public class Entity : MonoBehaviour
{
    public Animator EntityAnimator { get; private set; }
    public Rigidbody2D EntityRigidbody2D { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirectionValue { get; private set; } = 1;

    [Header("Collision detection")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform firstWallCheckTransform;
    [SerializeField] private Transform secondWallCheckTransform;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    protected virtual void Awake()
    {
        EntityAnimator = GetComponentInChildren<Animator>();
        EntityRigidbody2D = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        EntityRigidbody2D.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
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
        groundDetected = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, whatIsGround);
        if (secondWallCheckTransform != null)
        {
            wallDetected = Physics2D.Raycast(firstWallCheckTransform.position, Vector2.right * facingDirectionValue, wallCheckDistance, whatIsGround)
                && Physics2D.Raycast(secondWallCheckTransform.position, Vector2.right * facingDirectionValue, wallCheckDistance, whatIsGround);
        }
        else wallDetected = Physics2D.Raycast(firstWallCheckTransform.position, Vector2.right * facingDirectionValue, wallCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheckTransform.position, groundCheckTransform.position - new Vector3(0, groundCheckDistance));
            Gizmos.DrawLine(firstWallCheckTransform.position, firstWallCheckTransform.position + new Vector3(wallCheckDistance * facingDirectionValue, 0));
        if (secondWallCheckTransform != null)
        {
            Gizmos.DrawLine(secondWallCheckTransform.position, secondWallCheckTransform.position + new Vector3(wallCheckDistance * facingDirectionValue, 0));
        }
    }
}
