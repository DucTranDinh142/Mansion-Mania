using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpVelocity.x * -player.facingDirectionValue, player.wallJumpVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(rigidbody.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
        if(player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}