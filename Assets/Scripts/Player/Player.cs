using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    private UI ui;
    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX playerVFX { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    #region State Variables
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
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_ZaWarudoState zaWarudoState { get; private set; }
    #endregion

    [Header("Attack Stats")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration;
    public float comboResetTime;
    private Coroutine queuedAttackCoroutine;

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;


    [Header("Movement Stats")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpVelocity { get; private set; }
    [Range(0f, 1f)]
    public float moveInAirSpeedMultiplier;
    [Range(0f, 1f)]
    public float wallSlideSpeedMultiplier;
    [Space]
    public float dashDuration;
    public float dashSpeed;

    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();
        playerVFX = GetComponent<Player_VFX>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        health = GetComponent<Entity_Health>();

        #region States Machine Management
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
        swordThrowState = new Player_SwordThrowState(stateMachine, "SwordThrow", this);
        zaWarudoState = new Player_ZaWarudoState(stateMachine, "JumpFall", this);
        #endregion
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

        input.Player.Mouse.performed += context => mousePosition = context.ReadValue<Vector2>();
        //input.Player.Movement.started
        input.Player.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        input.Player.Movement.canceled += context => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += context => ui.ToggleSkillTreeUI();
        input.Player.Spell.performed += context => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += context => skillManager.timeEcho.TryUseSkill();
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

    public void TeleportPlayer(Vector3 position) => transform.position = position;
    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = entityAnimator.speed;
        Vector2 originalWallJump = wallJumpVelocity;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        entityAnimator.speed *= speedMultiplier;
        wallJumpVelocity *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        entityAnimator.speed = originalAnimSpeed;
        wallJumpVelocity = originalWallJump;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }

    }
    protected override void Update()
    {
        base.Update();
        wallJumpVelocity = new Vector2(moveSpeed * moveInAirSpeedMultiplier, jumpForce);
    }
}
