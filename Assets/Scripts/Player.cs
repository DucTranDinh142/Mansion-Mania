using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigid;
    private Animator playerAnimator;

    [Header("Attack details")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8;
    private float xInput;
    private bool facingRight = true;
    private int facingRightValue = 1;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageEnemies()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);
        foreach (Collider2D enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage();
        }
    }
    public void EnableMovements(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    private void HandleAnimations()
    {
        playerAnimator.SetFloat("xVelocity", playerRigid.linearVelocity.x);
        playerAnimator.SetFloat("yVelocity", playerRigid.linearVelocity.y);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W))
            TryToJump();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryToAttack();
    }
    private void TryToAttack()
    {
        if (isGrounded)
            playerAnimator.SetTrigger("Attack!");
    }

    private void HandleMovement()
    {
        if (canMove)
            playerRigid.linearVelocity = new Vector2(xInput * moveSpeed, playerRigid.linearVelocity.y);
        else
            playerRigid.linearVelocity = new Vector2(0, playerRigid.linearVelocity.y);
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            playerRigid.linearVelocity = new Vector2(playerRigid.linearVelocity.x, jumpForce);
    }
    private void HandleFlip()
    {
        if (playerRigid.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (playerRigid.linearVelocity.x < 0 && facingRight == true)
            Flip();
    }
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingRightValue *= -1;
    }
    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
