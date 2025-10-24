using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animatorBoolName;

    protected Animator animator;
    protected Rigidbody2D playerRigidbody;
    protected PlayerInputSet input;

    public EntityState(StateMachine stateMachine, string animatorBoolName, Player player)
    {
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
        this.player = player;

        animator = player.animator;
        playerRigidbody = player.playerRigidbody;
        input = player.input;
    }
    public virtual void Enter()
    {
        animator.SetBool(animatorBoolName, true);
    }
    public virtual void Update()
    {
        animator.SetFloat("YVelocity",playerRigidbody.linearVelocity.y);
    }
    public virtual void Exit()
    {
        animator.SetBool(animatorBoolName, false);
    }
}
