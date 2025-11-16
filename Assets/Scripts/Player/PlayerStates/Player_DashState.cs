using UnityEngine;

public class Player_DashState : PlayerState
{
    private float ogGravityScale;
    private int dashDirectionValue;
    public Player_DashState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.playerVFX.DoImageEchoEffect(player.dashDuration);

        dashDirectionValue = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirectionValue;
        stateTimer = player.dashDuration;
        ogGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
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
        else if (stateTimer < 0)
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
        
        skillManager.dash.OnEndEffect();

        player.SetVelocity(0,0);
        rigidbody.gravityScale = ogGravityScale;

        player.health.SetCanTakeDamage(true);
    }
}
