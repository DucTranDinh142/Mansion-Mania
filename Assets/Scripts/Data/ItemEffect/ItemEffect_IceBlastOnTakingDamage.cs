using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Elemental Blast/Ice Blast", fileName = "Item Effect data - Ice Blast - ")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float hpPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    [NonSerialized] private float lastTimeUsed = -999;
    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVFX;
    [SerializeField] private GameObject onHitVFX;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHPPercent() <= hpPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.playerVFX.CreateEffectOf(iceBlastVFX, player.transform); 
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }
    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach(var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if(targetGotHit)
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