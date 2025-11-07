public class Player_AiredState : PlayerState
{
    public Player_AiredState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * (player.moveInAirSpeedMultiplier * player.moveSpeed), rigidbody.linearVelocity.y);

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);

        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}