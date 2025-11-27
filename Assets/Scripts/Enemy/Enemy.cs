using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Entity_Stats entityStats { get; private set; }

    public Enemy_Health health { get; private set; }
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_SurprisedState surprisedState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("Base Reward on Killed")]
    [SerializeField] private int gold;
    [SerializeField] private int skillPoint;
    [Header("Battle Phase Stats")]
    public float surprisedTimer;
    public float battleMoveSpeed;
    public float attackDistance;
    [Range(0f, 3f)]
    public float battleAnimSpeedMultipier;
    public float battleTimeDuration;
    public float minRetreatDistance;
    public Vector2 retreatVelocity;

    [Header("Got Stunned Detail")]
    public float stunnedDuration;
    public Vector2 stunnedVelocity;
    [SerializeField] protected bool canBeStunned;

    [Header("Movement Stats")]
    public float idleTime;
    public float moveSpeed;
    [Range(0f, 2f)]
    public float moveAnimSpeedMultipier;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckDistance;
    public Transform player { get; private set; }
    public float activeSlowMultiplier { get; private set; } = 1;

    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier;
    public float GetBattleSpeed() => battleMoveSpeed * activeSlowMultiplier;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Enemy_Health>();
        entityStats = GetComponent<Entity_Stats>();

    }
    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {

        activeSlowMultiplier = 1 - slowMultiplier;
        entityAnimator.speed *= activeSlowMultiplier;

        yield return new WaitForSeconds(duration);
        StopSlowDown();
    }
    public override void StopSlowDown()
    {
        activeSlowMultiplier = 1;
        entityAnimator.speed = 1;
        base.StopSlowDown();

    }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable; 
    public override void EntityDeath()
    {
        Player player = FindFirstObjectByType<Player>();
        player.inventory.gold += gold;
        player.ui.skillTreeUI.AddSkillPoints(skillPoint);
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    public void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState) return;

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;

        return player;
    }

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
    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
