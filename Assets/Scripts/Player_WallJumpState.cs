using UnityEngine;

public class Player_WallJumpState : EntityState
{
    public Player_WallJumpState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpDirection.x * -player.facingDirectionValue, player.wallJumpDirection.y);
    }
    public override void Update()
    {
        base.Update();
        if(playerRigidbody.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
        if(player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}