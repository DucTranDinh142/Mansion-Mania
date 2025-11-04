public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;

    public PlayerState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName)
    {
        this.player = player;

        animator = player.EntityAnimator;
        rigidbody = player.EntityRigidbody2D;
        input = player.input;
    }
    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);

    }
    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        animator.SetFloat("YVelocity", rigidbody.linearVelocity.y);
    }
    private bool CanDash()
    {
        if (player.wallDetected)
            return false;
        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
