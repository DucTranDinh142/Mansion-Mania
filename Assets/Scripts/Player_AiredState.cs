using UnityEngine;

public class Player_AiredState : EntityState
{
    public Player_AiredState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.moveInput.x * (player.moveInJumpSpeedMultiplier * player.moveSpeed), playerRigidbody.linearVelocity.y);
    }
}