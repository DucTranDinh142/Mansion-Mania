using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();

        if (ShouldRetreat())
        {
            rigidbody.linearVelocity = new Vector2(enemy.retreatVelocity.x * enemy.activeSlowMultiplier * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
        else enemy.SetVelocity(0, rigidbody.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else enemy.SetVelocity(enemy.GetBattleSpeed() * DirectionToPlayer(), rigidbody.linearVelocity.y);
        if (enemy.wallDetected)
            enemy.SetVelocity(0, rigidbody.linearVelocity.y);
    }
    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false) return;
        Transform newTarget = enemy.PlayerDetected().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }
    protected void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    protected bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    public bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;
    protected float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
    public override void Exit()
    {
        base.Exit();
        if (ShouldRetreat() == false)
            enemy.SetVelocity(0, rigidbody.linearVelocity.y);
    }
}
