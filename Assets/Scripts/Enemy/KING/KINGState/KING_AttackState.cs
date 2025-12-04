public class KING_AttackState : EnemyState
{
    private KING king;
    public KING_AttackState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
        king = enemy as KING;
    }
    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (king.ShouldTeleport())
                stateMachine.ChangeState(king.kingTeleportState);
            else
                stateMachine.ChangeState(king.kingBattleState);
        }
    }
}
