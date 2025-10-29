using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animatorBoolName;

    protected Animator animator;
    protected Rigidbody2D playerRigidbody;
    protected PlayerInputSet input;

    protected float StateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animatorBoolName, Player player)
    {
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
        this.player = player;

        animator = player.playerAnimator;
        playerRigidbody = player.playerRigidbody;
        input = player.input;
    }
    public virtual void Enter()
    {
        animator.SetBool(animatorBoolName, true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;
        animator.SetFloat("YVelocity", playerRigidbody.linearVelocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }
    public virtual void Exit()
    {
        animator.SetBool(animatorBoolName, false);
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
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
