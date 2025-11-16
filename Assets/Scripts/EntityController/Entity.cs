using System;
using System.Collections;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public event Action Onflipped;


    public Animator entityAnimator { get; private set; }
    public Rigidbody2D entityRigidbody2D { get; private set; }
    public Entity_Stats entityStats { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirectionValue { get; private set; } = 1;

    [Header("Collision detection")]
    public LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform firstWallCheckTransform;
    [SerializeField] private Transform secondWallCheckTransform;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool isKnockbacked;
    private Coroutine knockbackCoroutine;
    private Coroutine slowDownCoroutine;

    protected virtual void Awake()
    {
        entityAnimator = GetComponentInChildren<Animator>();
        entityRigidbody2D = GetComponent<Rigidbody2D>();
        entityStats = GetComponent<Entity_Stats>();

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
    public virtual void EntityDeath()
    {

    }
    public virtual void SlowDownEntity(float duration, float slowMultiplier, bool canOverrideSlowEffect = false)
    {
        if (slowDownCoroutine != null)
        {
            if (canOverrideSlowEffect)
                StopCoroutine(slowDownCoroutine);
            else
                return;
        }

        slowDownCoroutine = StartCoroutine(SlowDownEntityCoroutine(duration, slowMultiplier));
    }
    protected virtual IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public virtual void StopSlowDown()
    {
        slowDownCoroutine = null;
    }
    public void RecieveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockback, duration));
    }
    private IEnumerator KnockbackCoroutine(Vector2 knockback, float duration)
    {
        isKnockbacked = true;
        entityRigidbody2D.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        entityRigidbody2D.linearVelocity = Vector2.zero;
        isKnockbacked = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnockbacked) return;

        entityRigidbody2D.linearVelocity = new Vector2(xVelocity, yVelocity);
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

        Onflipped?.Invoke();
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
