using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    [Header("Battle Phase Stats")]
    public float battleMoveSpeed;
    public float attackDistance;
    [Range(0f, 3f)]
    public float battleAnimSpeedMultipier;
    public float battleTimeDuration;
    public float minRetreatDistance;
    public Vector2 retreatVelocity;

    [Header("Movement Stats")]
    public float idleTime;
    public float moveSpeed;
    [Range(0f, 2f)]
    public float moveAnimSpeedMultipier;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckDistance;


    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(playerCheckTransform.position, Vector2.right * facingDirectionValue, playerCheckDistance, whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(playerCheckTransform.position, new Vector3(playerCheckTransform.position.x + (facingDirectionValue * playerCheckDistance), playerCheckTransform.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheckTransform.position, new Vector3(playerCheckTransform.position.x + (facingDirectionValue * attackDistance), playerCheckTransform.position.y));
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(playerCheckTransform.position, new Vector3(playerCheckTransform.position.x + (facingDirectionValue * minRetreatDistance), playerCheckTransform.position.y));


    }
}
