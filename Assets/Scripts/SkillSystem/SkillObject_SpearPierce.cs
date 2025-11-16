using UnityEngine;

public class SkillObject_SpearPierce : SkillObject_Sword
{
    private int amountToPierce;

    public override void SetUpSword(Skill_SwordThrow_D swordManager, Vector2 direction)
    {
        base.SetUpSword(swordManager, direction);
        amountToPierce = swordManager.amountToPierce;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amountToPierce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform, checkRadius, swordManager);
            CheckEnableCollision(collision);
            StopSword(collision);
            return;
        }
        DamageEnemiesInRadius(transform, checkRadius, swordManager);
        amountToPierce--;

    }
}
