using UnityEngine;

public class Enemy_DeadState : EnemyState
{

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        animator.enabled = false;
        enemy.GetComponent<Collider2D>().enabled = false;

        rigidbody.gravityScale = 12;
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 15);

        stateMachine.SwitchOffStateMachine();
    }

}
