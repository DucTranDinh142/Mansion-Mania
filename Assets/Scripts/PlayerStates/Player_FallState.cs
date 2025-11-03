using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Update()
    {
        base.Update();

        if (player.groundDetected == true)
            stateMachine.ChangeState(player.idleState);
    }
}
