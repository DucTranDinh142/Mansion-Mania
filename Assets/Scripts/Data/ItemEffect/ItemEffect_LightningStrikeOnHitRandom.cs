using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Thunder Strike On Hit Random",
    fileName = "Item Effect - Thunder Strike On Hit Random")]
public class ItemEffect_ThunderStrikeOnHit : ItemEffectDataSO
{
    [Header("Thunder Strike Stats")]
    [SerializeField] private float triggerChance = 0.15f;   // 15%
    [SerializeField] private float lightningDamage = 200f;
    [SerializeField] private ElementalEffectData effectData;

    [Header("VFX")]
    [SerializeField] private GameObject thunderVFX;

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.combat.OnHitTarget += TryThunderStrike;
    }

    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.combat.OnHitTarget -= TryThunderStrike;
        player = null;
    }

    private void TryThunderStrike(Transform target, float damageDealt)
    {
        if (Random.value > triggerChance)
            return;

        IDamagable dmg = target.GetComponent<IDamagable>();
        if (dmg != null)
        {
            dmg.TakeDamage(0, lightningDamage, ElementType.Lightning, player.transform);
        }

        var handler = target.GetComponent<Entity_StatusHandler>();
        handler?.ApplyStatusEffect(ElementType.Lightning, effectData);

        if (thunderVFX != null)
            player.playerVFX.CreateEffectOf(thunderVFX, target.transform);
    }
}
