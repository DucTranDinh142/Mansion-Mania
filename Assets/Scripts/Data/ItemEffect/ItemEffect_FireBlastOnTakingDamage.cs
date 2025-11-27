using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Elemental Blast/Fire Blast",
    fileName = "Item Effect data - Fire Blast - ")]
public class ItemEffect_FireBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float fireDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float hpPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    [NonSerialized] private float lastTimeUsed = -999;

    [Header("Vfx Objects")]
    [SerializeField] private GameObject fireBlastVFX;
    [SerializeField] private GameObject onHitVFX;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHPPercent() <= hpPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.playerVFX.CreateEffectOf(fireBlastVFX, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithFire();
        }
    }

    private void DamageEnemiesWithFire()
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
                fireDamage,
                ElementType.Fire,
                player.transform
            );

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Fire, effectData);

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
