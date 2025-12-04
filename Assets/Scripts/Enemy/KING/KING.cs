using System.Collections;
using UnityEngine;

public class KING : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public KING_AttackState kingAttackState { get; private set; }
    public KING_BattleState kingBattleState { get; private set; }
    public KING_TeleportState kingTeleportState { get; private set; }
    public KING_SpellCastState kingSpellCastState { get; private set; }
    public KING_DeadState kingDeadState { get; private set; }

    [Header("KING Specifics")]
    public float maxBattleIdleTime = 5;

    [Header("KING Spellcast")]
    [SerializeField] private GameObject spellCastPrefab;
    [SerializeField] private int amountToCast = 6;
    [SerializeField] private float spellCastRate = 1.2f;
    [SerializeField] private float spellCastStateCooldown = 10f;
    [SerializeField] private Vector2 playerOffsetPrediction;
    private float lastTimeCastedSpells = float.NegativeInfinity;
    public bool spellCastPerformed { get; private set; }
    private Player playerScript;

    [Header("KING Teleport")]
    [SerializeField] private BoxCollider2D arenaBounds;
    [SerializeField] private float offsetCenterY = 0.6f;
    [SerializeField] private float chanceToTeleport = .20f;
    private float defaultTeleportChance;
    public bool teleportTrigger {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "Idle");
        moveState = new Enemy_MoveState(this, stateMachine, "Move");
        surprisedState = new Enemy_SurprisedState(this, stateMachine, "Surprised");
        deadState = new Enemy_DeadState(this, stateMachine, "Surprised");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "Stunned");

        kingBattleState = new KING_BattleState(this, stateMachine, "Battle");
        kingAttackState = new KING_AttackState(this, stateMachine, "Attack");
        kingTeleportState = new KING_TeleportState(this, stateMachine, "Teleport");
        kingSpellCastState = new KING_SpellCastState(this, stateMachine, "SpellCast");
        kingDeadState = new KING_DeadState(this, stateMachine, "Surprised");

        battleState = kingBattleState;
    }
    protected override void Start()
    {
        base.Start();

        arenaBounds.transform.parent = null;
        defaultTeleportChance = chanceToTeleport;

        stateMachine.Initialize(idleState);
    }
    public void HandleCounter()
    {
        if (CanBeCountered == false) return;

        stateMachine.ChangeState(stunnedState);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SpecialAttack()
    {
        StartCoroutine(CastSpellCo());
    }
    public override void EntityDeath()
    {
        stateMachine.ChangeState(kingDeadState);
    }
    public override void TryEnterBattleState(Transform player)
    {
        if(stateMachine.currentState == kingSpellCastState) return;
        base.TryEnterBattleState(player);
    }
    private IEnumerator CastSpellCo()
    {
        if (playerScript == null)
            playerScript = player.GetComponent<Player>();

        for (int i = 0; i < amountToCast; i++)
        {
            bool playerMoving = playerScript.entityRigidbody2D.linearVelocity.magnitude > 0;

            float xOffset = playerMoving ? playerOffsetPrediction.x * playerScript.facingDirectionValue : 0;
            Vector3 spellPosition = player.transform.position + new Vector3(xOffset, playerOffsetPrediction.y);

            KING_Spell projectile
                = Instantiate(spellCastPrefab, spellPosition, Quaternion.identity)
                    .GetComponent<KING_Spell>();

            projectile.SetupSpell(entityCombat);
            yield return new WaitForSeconds(spellCastRate);
        }
        SetSpellCastPerformed(true);
    }

    public void SetSpellCastPerformed(bool spellCastStatus) => spellCastPerformed = spellCastStatus;
    public bool CanDoSpellCast() => Time.time > lastTimeCastedSpells + spellCastStateCooldown;
    public void SetSpellCastOnCooldown() => lastTimeCastedSpells = Time.time;
    public bool ShouldTeleport()
    {
        if(Random.value >= chanceToTeleport)
        {
            chanceToTeleport += .05f;
            return false;
        }
        chanceToTeleport = defaultTeleportChance;
        return true;
    }

    public void SetTeleportTrigger(bool triggerStatus) => teleportTrigger = triggerStatus;

    public Vector3 FindTeleportPoint()
    {
        int maxAttempts = 10;
        float bossWidthColliderHalf = entityCollider2D.bounds.size.x/2;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(arenaBounds.bounds.min.x + bossWidthColliderHalf +.01f,
                                         arenaBounds.bounds.max.x -( bossWidthColliderHalf + .01f));
            Vector2 raycastPoint = new Vector2(randomX, arenaBounds.bounds.max.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down, Mathf.Infinity, whatIsGround);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offsetCenterY);
        }

        return transform.position;
    }
}
