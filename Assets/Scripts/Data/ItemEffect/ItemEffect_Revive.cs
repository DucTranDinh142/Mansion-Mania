using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Revive",
    fileName = "Item Effect - Revive")]
public class ItemEffect_Revive : ItemEffectDataSO
{
    [SerializeField] private float revivePercent = 0.5f;
    [SerializeField] private float cooldown = 360;
    [NonSerialized] private float lastTimeUsed = -999;
    [Header("Vfx Objects")]
    [SerializeField] private GameObject reviveVFX;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHPPercent() <= 0;
        if (noCooldown & reachedThreshold)
        {
            player.playerVFX.CreateEffectOf(reviveVFX,player.transform);
            lastTimeUsed = Time.time;
            RevivePlayer();
        }
    }
    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.health.OnAboutToDie += ExecuteEffect;
    }
    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.health.OnAboutToDie -= ExecuteEffect;
        player = null;
    }
    private void RevivePlayer()
    {
        player.health.IncreaseHP(player.stats.GetMaxHP() * revivePercent);
    }
}
