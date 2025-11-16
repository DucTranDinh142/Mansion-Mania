
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float totalBurnDamage;
    public int burnTickPerSec;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entityStats, ScaleData scaleData)
    {
        chillDuration = scaleData.chillDuration;
        chillSlowMultiplier = scaleData.chillSlowMultiplier;

        burnDuration = scaleData.burnDuration;
        totalBurnDamage = entityStats.offensiveStat.fireDamage.GetValue() * scaleData.burnDamageScale;
        burnTickPerSec = scaleData.burnTickPerSec;

        shockDuration = scaleData.shockDuration;
        shockDamage = entityStats.offensiveStat.lightningDamage.GetValue() * scaleData.shockDamageScale;
        shockCharge = scaleData.shockCharge;
    }
}
