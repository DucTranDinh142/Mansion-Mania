using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow_D swordManager;


    protected Transform playerTransform;
    protected bool shouldComeBack;
    protected float comebackSpeed = 36;
    protected float maxAllowedDistance = 22;

    protected virtual void Update()
    {
        transform.right = skillObjectRigidbody.linearVelocity;
        HandleComeback();
    }

    public virtual void SetUpSword(Skill_SwordThrow_D swordManager, Vector2 direction)
    {
        
        skillObjectRigidbody.linearVelocity = direction;
        this.swordManager = swordManager;

        playerTransform = swordManager.transform.root;
        playerStats = swordManager.player.entityStats;
        scaleData = swordManager.scaleData;
    }
    public void GetSwordBackToPlayer() => shouldComeBack = true;

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if(distance > maxAllowedDistance)
        {
            GetSwordBackToPlayer();
        }

        if (shouldComeBack == false) return;


        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed*Time.deltaTime);

        if(distance < .5f )
            Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        DamageEnemiesInRadius(transform, checkRadius, swordManager);
        CheckEnableCollision(collision);
        StopSword(collision);
    }

    protected void CheckEnableCollision(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().enabled == false)
            GetSwordBackToPlayer();
    }

    protected void StopSword(Collider2D collision)
    {
        skillObjectRigidbody.simulated = false;
        transform.parent = collision.transform;
    }
}
