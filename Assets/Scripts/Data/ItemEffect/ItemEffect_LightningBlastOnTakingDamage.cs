using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Elemental Blast/Lightning Blast",
    fileName = "Item Effect data - Lightning Blast - ")]
public class ItemEffect_LightningBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float lightningDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float hpPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    [NonSerialized] private float lastTimeUsed = -999;

    [Header("Vfx Objects")]
    [SerializeField] private GameObject lightningBlastVFX;
    [SerializeField] private GameObject onHitVFX;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHPPercent() <= hpPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.playerVFX.CreateEffectOf(lightningBlastVFX, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithLightning();
        }
    }

    private void DamageEnemiesWithLightning()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            player.transform.position,
            1.5f,
            whatIsEnemy
        );

        foreach (var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(
                0,
                lightningDamage,
                ElementType.Lightning,
                player.transform
            );

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Lightning, effectData);

            if (targetGotHit)
                player.playerVFX.CreateEffectOf(onHitVFX, target.transform);
        }
    }

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
