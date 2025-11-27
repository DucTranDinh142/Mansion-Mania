using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Buff Effect", fileName = "Item Effect data - Buff - ")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    public override bool CanBeUsed(Player player)
    {

        if (player.stats.CanApplyBuffOf(source))
        {
            return true;
        }
        else
            return false;
    }
    public override void ExecuteEffect()
    {
        player.stats.ApplyBuff(buffsToApply, duration, source);
    }
}
