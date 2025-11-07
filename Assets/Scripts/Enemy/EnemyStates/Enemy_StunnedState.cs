using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX enemy_VFX;
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
        enemy_VFX = enemy.GetComponent<Enemy_VFX>();
    }
    public override void Enter()
    {
        base.Enter();

        enemy_VFX.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);

        stateTimer = enemy.stunnedDuration;
        rigidbody.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDirectionValue, enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.battleState);
    }
}
