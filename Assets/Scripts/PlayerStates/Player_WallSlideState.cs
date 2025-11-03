public class Player_WallSlideState : EntityState
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
            stateMachine.ChangeState(player.dashState);
            HandleFlip();
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
            player.SetVelocity(player.moveInput.x, playerRigidbody.linearVelocity.y);
        else
            player.SetVelocity(player.moveInput.x, playerRigidbody.linearVelocity.y * player.wallSlideSpeedMultiplier);
    }

}
