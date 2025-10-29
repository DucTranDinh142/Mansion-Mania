using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Update()
    {
        base.Update();
        if (playerRigidbody.linearVelocity.y < 0 && player.groundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);
        if (input.Player.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
    }
}