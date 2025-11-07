public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() && stateMachine.currentState != enemy.battleState)
        {
            if (enemy.surprisedTimer <= 0)
                stateMachine.ChangeState(enemy.battleState);
            else
                stateMachine.ChangeState(enemy.surprisedState);
        }

    }
}
