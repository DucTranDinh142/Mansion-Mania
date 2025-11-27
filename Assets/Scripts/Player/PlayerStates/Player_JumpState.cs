using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rigidbody.linearVelocity.x, player.jumpForce);
        player.entitySFX?.Jumping();
    }
    public override void Update()
    {
        base.Update();
        if (rigidbody.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
    public override void Exit()
    {
        base.Exit();
        player.entitySFX?.StopVFX();
    }
}
