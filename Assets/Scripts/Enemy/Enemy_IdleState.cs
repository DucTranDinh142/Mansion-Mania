public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, rigidbody.linearVelocity.y);
        StateTimer = enemy.idleTime;
    }
    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
