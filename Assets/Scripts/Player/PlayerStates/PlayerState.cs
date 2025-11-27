public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;

    public PlayerState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName)
    {
        this.player = player;

        animator = player.entityAnimator;
        rigidbody = player.entityRigidbody2D;
        stats = player.stats;
        input = player.input;
        skillManager = player.skillManager;
    }
    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCoolDown();
            stateMachine.ChangeState(player.dashState);
        }
        if (input.Player.Ultimate.WasPressedThisFrame() && skillManager.ultimate.CanUseSkill())
        {
            player.entitySFX.Ultimate();
            if (skillManager.ultimate.InstantDomain())
                skillManager.ultimate.CreateDomain();
            else stateMachine.ChangeState(player.zaWarudoState);

            skillManager.ultimate.SetSkillOnCoolDown();
        }

    }
    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        animator.SetFloat("YVelocity", rigidbody.linearVelocity.y);
    }
    private bool CanDash()
    {


        if (skillManager.dash.CanUseSkill()==false) 
            return false;
        if (player.wallDetected)
            return false;
        if (stateMachine.currentState == player.dashState || stateMachine.currentState == player.zaWarudoState)
            return false;

        return true;
    }
}
