using UnityEngine;

public class KING_DeadState : Enemy_DeadState
{
    public KING_DeadState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        Player.instance.ui.OpenWinUI();
        base.Enter();
    }
}
