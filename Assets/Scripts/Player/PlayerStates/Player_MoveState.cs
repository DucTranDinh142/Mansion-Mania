using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(StateMachine stateMachine, string stateName, Player player) : base(stateMachine, stateName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.entitySFX.Moving();
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rigidbody.linearVelocity.y);

        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.entitySFX.StopVFX();
    }
}
