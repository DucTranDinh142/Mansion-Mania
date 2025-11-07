using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }

    [Header("Attack Stats")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration;
    public float comboResetTime;
    private Coroutine queuedAttackCoroutine;

    [Header("Movement Stats")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpDirection { get; private set; }
    [Range(0f, 1f)]
    public float moveInAirSpeedMultiplier;
    [Range(0f, 1f)]
    public float wallSlideSpeedMultiplier;
    [Space]
    public float dashDuration;
    public float dashSpeed;

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();

        idleState = new Player_IdleState(stateMachine, "Idle", this);
        moveState = new Player_MoveState(stateMachine, "Move", this);
        jumpState = new Player_JumpState(stateMachine, "JumpFall", this);
        fallState = new Player_FallState(stateMachine, "JumpFall", this);
        wallSlideState = new Player_WallSlideState(stateMachine, "WallSlide", this);
        wallJumpState = new Player_WallJumpState(stateMachine, "JumpFall", this);
        dashState = new Player_DashState(stateMachine, "Dash", this);
        basicAttackState = new Player_BasicAttackState(stateMachine, "BasicAttack", this);
        jumpAttackState = new Player_JumpAttackState(stateMachine, "JumpAttack", this);
        deadState = new Player_DeadState(stateMachine, "Dead", this);
        counterAttackState = new Player_CounterAttackState(stateMachine, "CounterAttack", this);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }
    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);
        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCoroutine());
    }
    private IEnumerator EnterAttackStateWithDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    private void OnEnable()
    {
        input.Enable();
        //input.Player.Movement.started
        input.Player.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        input.Player.Movement.canceled += context => moveInput = Vector2.zero;
    }
    private void OnDisable()
    {
        input.Disable();
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        wallJumpDirection = new Vector2(moveSpeed * moveInAirSpeedMultiplier, jumpForce);
    }
}
