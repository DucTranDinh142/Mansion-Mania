using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(stateMachine, animatorBoolName)
    {
        this.enemy = enemy;

        animator = enemy.EntityAnimator;
        rigidbody = enemy.EntityRigidbody2D;
    }
    public override void Update()
    {
        base.Update();
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        animator.SetFloat("MoveAnimSpeedMultipier", enemy.moveAnimSpeedMultipier);
        animator.SetFloat("BattleAnimSpeedMultipier", enemy.battleAnimSpeedMultipier);
        animator.SetFloat("xVelocity", rigidbody.linearVelocity.x);
    }
}
