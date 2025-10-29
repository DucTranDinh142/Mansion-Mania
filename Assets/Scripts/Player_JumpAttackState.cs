public class Player_JumpAttackState : EntityState
{
    private bool touchedGround;

    public Player_JumpAttackState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        touchedGround = false;
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDirectionValue, player.jumpAttackVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            animator.SetTrigger("JumpAttackTrigger");
            player.SetVelocity(0, playerRigidbody.linearVelocity.y);
        }

        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}