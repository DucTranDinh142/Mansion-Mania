public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(StateMachine stateMachine, string stateName, Player player) : base(stateMachine, stateName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rigidbody.linearVelocity.y);
        player.entitySFX?.StopVFX();
    }
    public override void Update()
    {
        base.Update();
        if (player.moveInput.x == player.facingDirectionValue && player.wallDetected)
            return;

        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
    }
}
