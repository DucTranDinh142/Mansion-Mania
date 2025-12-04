using UnityEngine;

public class KING_BattleState : Enemy_BattleState
{
    private KING king;
    public KING_BattleState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
        king = enemy as KING;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = king.maxBattleIdleTime;
    }
    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (stateTimer < 0)
            stateMachine.ChangeState(king.kingTeleportState);

        if (enemy.PlayerDetected())
            UpdateTargetIfNeeded();


        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(king.kingAttackState);

        else
        {
            float xVelocity = enemy.groundDetected ? enemy.GetBattleSpeed() : 0.0001f;
            enemy.SetVelocity(xVelocity * DirectionToPlayer(), rigidbody.linearVelocity.y);
        }

        if (enemy.wallDetected)
            enemy.SetVelocity(0, rigidbody.linearVelocity.y);
    }
}
