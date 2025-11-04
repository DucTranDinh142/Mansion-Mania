using UnityEngine;

public class SkeletonSwordman : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "Idle");
        moveState = new Enemy_MoveState(this, stateMachine, "Move");
        attackState = new Enemy_AttackState(this, stateMachine, "Attack");
        battleState = new Enemy_BattleState(this, stateMachine, "Battle");
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
}
