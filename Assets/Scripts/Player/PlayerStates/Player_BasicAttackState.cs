using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private bool comboAttackQueued;
    private int attackDirection;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private const int DefaultComboIndex = 1;

    public Player_BasicAttackState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
        if (comboLimit != player.attackVelocity.Length)
            comboLimit = player.attackVelocity.Length;
    }
    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndex();
        SyncAttackSpeed();

        attackDirection = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDirectionValue;

        //if (player.moveInput.x != 0)
        //    attackDirection = ((int)player.moveInput.x);
        //else
        //    attackDirection = player.facingDirectionValue;

        animator.SetInteger("BasicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();
        if (triggerCalled)
            HandleStateExit();
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            animator.SetBool(animatorBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }
    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rigidbody.linearVelocity.y);
    }
    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
    private void ResetComboIndex()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = DefaultComboIndex;
    }
}