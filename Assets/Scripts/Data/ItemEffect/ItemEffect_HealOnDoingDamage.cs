using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal On Damage Effect", fileName = "Item Effect data - Heal On Damage - ")]
public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealedOnAttack = .3f;

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }
    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }
    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHP(damage * percentHealedOnAttack);
    }
}
