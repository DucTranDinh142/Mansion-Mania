public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.groundDetected == false || enemy.wallDetected)
            enemy.Flip();
    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.GetMoveSpeed() * enemy.facingDirectionValue, rigidbody.linearVelocity.y);

        if (enemy.groundDetected == false || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
}
