using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVFX;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer;

    private Transform playerTransform;
    private Skill_TimeEcho_A echoManager;
    private TrailRenderer wispTrail;
    private Entity_Health playerHealth;
    private SkillObject_Health echoHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler playerStatusHandler;

    public int maxAttacks { get; private set; }
    public void SetupEcho(Skill_TimeEcho_A echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.entityStats;
        scaleData = echoManager.scaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        echoHealth = GetComponent<SkillObject_Health>();
        skillManager = echoManager.skillManager;
        playerStatusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
        FlipToTarget();

        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        animator.SetBool("Attack", maxAttacks > 0);
    }
    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            animator.SetFloat("YVelocity", skillObjectRigidbody.linearVelocity.y);
            StopHorizontalMovement();

        }

    }
    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, playerTransform.position) < .75f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
        playerHealth.IncreaseHP(echoHealth.lastDamageTaken*echoManager.GetPercentOfDamageHealed());

        float cooldownToReduce = echoManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(cooldownToReduce);

        if (echoManager.CanRemoveNegativeEffect())
            playerStatusHandler.RemoveAllNegativeEffect();
    }

    private void FlipToTarget()
    {
        Transform target = ClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }
    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheckTransform, checkRadius, echoManager);
        if (targetGotHit == false) return;
        bool canDulicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDulicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }
    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
        {
            TurnedIntoWisp();
        }
        else
            Destroy(gameObject);
    }

    private void TurnedIntoWisp()
    {
        shouldMoveToPlayer = true;
        animator.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        skillObjectRigidbody.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            skillObjectRigidbody.linearVelocity = new Vector2(0, skillObjectRigidbody.linearVelocity.y);
    }
}
