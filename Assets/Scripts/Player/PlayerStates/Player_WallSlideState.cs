public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();
        if (input.Player.Dash.WasPressedThisFrame())
        {
            HandleFlip();
            stateMachine.ChangeState(player.dashState);
        }
        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);
        if (player.wallDetected == false)
            stateMachine.ChangeState(player.fallState);
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            HandleFlip();
        }
    }

    private void HandleFlip()
    {
        if (player.facingDirectionValue != player.moveInput.x)
            player.Flip();
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
            player.SetVelocity(player.moveInput.x, rigidbody.linearVelocity.y);
        else
            player.SetVelocity(player.moveInput.x, rigidbody.linearVelocity.y * player.wallSlideSpeedMultiplier);
    }

}
