using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Update()
    {
        base.Update();
        if (rigidbody.linearVelocity.y < 0 && player.groundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);
        if(input.Player.Counter.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
        if (input.Player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
            stateMachine.ChangeState(player.swordThrowState);
    }
}