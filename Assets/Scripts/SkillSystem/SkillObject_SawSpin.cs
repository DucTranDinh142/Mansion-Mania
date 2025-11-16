using UnityEngine;

public class SkillObject_SawSpin : SkillObject_Sword
{
    private int maxDistance;
    private float attackPerSecond;
    private float attackTimer;

    public override void SetUpSword(Skill_SwordThrow_D swordManager, Vector2 direction)
    {
        base.SetUpSword(swordManager, direction);

        animator?.SetTrigger("Spin");

        maxDistance = swordManager.maxDistance;
        attackPerSecond = swordManager.attackPerSecond;

        Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
    }
    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }
    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if( distanceToPlayer > maxDistance && skillObjectRigidbody.simulated == true)
            skillObjectRigidbody.simulated = false;
    }
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if(attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, checkRadius, swordManager);
            attackTimer = 1 / attackPerSecond;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        skillObjectRigidbody.simulated = false ;
    }
    
}
