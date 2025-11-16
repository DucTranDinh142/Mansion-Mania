using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animatorBoolName;

    protected Animator animator;
    protected Rigidbody2D rigidbody;
    protected Entity_Stats stats;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState (StateMachine stateMachine, string animatorBoolName)
    {
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
    }
    public virtual void Enter()
    {
        animator.SetBool(animatorBoolName, true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }
    public virtual void Exit()
    {
        animator.SetBool(animatorBoolName, false);
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void UpdateAnimationParameters()
    {

    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offensiveStat.attackSpeed.GetValue();
        animator.SetFloat("AttackSpeedMultiplier", attackSpeed);
    }
}
