using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();

        input.Disable();
        rigidbody.simulated = false;
    }
}
