using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal Effect", fileName = "Item Effect data - Heal - ")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = .1f;
    public override void ExecuteEffect()
    {
        Player player = FindAnyObjectByType<Player>();

        float healAmount = player.stats.GetMaxHP() * healPercent;

        player.health.IncreaseHP(healAmount);
    }
}
