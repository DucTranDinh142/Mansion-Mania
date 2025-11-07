using UnityEngine;

public class Enemy_SurprisedState : EnemyState
{
    public Enemy_SurprisedState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.surprisedTimer;
    }
    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetected() == false)
            stateMachine.ChangeState(enemy.idleState);
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.battleState);
    }
}
