using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(playerRigidbody.linearVelocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (playerRigidbody.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
