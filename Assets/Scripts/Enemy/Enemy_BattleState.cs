using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player == null)
            player = enemy.PlayerDetected().transform;

        if (ShouldRetreat())
        {
            rigidbody.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
        else enemy.SetVelocity(0, rigidbody.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetected())
            UpdateBattleTimer();
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rigidbody.linearVelocity.y);
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    public bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;
    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    private int DirectionToPlayer()
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
