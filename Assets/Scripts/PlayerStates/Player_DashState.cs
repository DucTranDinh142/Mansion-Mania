using UnityEngine;

public class Player_DashState : EntityState
{
    private float ogGravityScale;
    private int dashDirectionValue;
    public Player_DashState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        dashDirectionValue = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirectionValue;
        StateTimer = player.dashDuration;
        ogGravityScale = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * dashDirectionValue, 0);

        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else stateMachine.ChangeState(player.wallSlideState);
        }
        else if (StateTimer < 0)
        {
            if(player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit() 
    {
        base.Exit();
        player.SetVelocity(0,0);
        playerRigidbody.gravityScale = ogGravityScale;
    }
}
