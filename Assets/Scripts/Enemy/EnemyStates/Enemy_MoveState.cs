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

        enemy.entitySFX?.Moving();
    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.GetMoveSpeed() * enemy.facingDirectionValue, rigidbody.linearVelocity.y);

        if (enemy.groundDetected == false || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.entitySFX.StopVFX();
    }
}
