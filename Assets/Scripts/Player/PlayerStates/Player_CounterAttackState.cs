public class Player_CounterAttackState : PlayerState
{
    private Player_Combat player_Combat;
    private bool counteredSomebody;
    public Player_CounterAttackState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
        player_Combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player_Combat.GetCounterRecoveryDuration();
        counteredSomebody = player_Combat.CounterAttackPerformed();

        animator.SetBool("CounterAttackPerformed", counteredSomebody);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rigidbody.linearVelocity.y);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && counteredSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
