using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentElementEffect = ElementType.None;

    [Header("Shock Effect Details")]
    [SerializeField] private GameObject lightningStrikeVFX;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximunCharge = 1;
    private Coroutine shockCoroutine;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public void RemoveAllNegativeEffect()
    {
        StopAllCoroutines();
        currentElementEffect = ElementType.None;
        entityVFX.StopAllVFX();
    }
    public void ApplyStatusEffect(ElementType element, ElementalEffectData elementalEffectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillEffect(elementalEffectData.chillDuration, elementalEffectData.chillSlowMultiplier);

        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(elementalEffectData.burnDuration, elementalEffectData.totalBurnDamage, elementalEffectData.burnTickPerSec);

        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(elementalEffectData.shockDuration, elementalEffectData.shockDamage, elementalEffectData.shockCharge);
            

    }
    public void ApplyShockEffect(float duration, float damage, float charge)
    {
        float lightningResistance = entityStats.GetElementalReistance(ElementType.Lightning);
        float finalcharge = charge * (1 - lightningResistance);
        currentCharge += charge;

        if(currentCharge >= maximunCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }

        if (shockCoroutine != null)
            StopCoroutine(shockCoroutine);

        shockCoroutine = StartCoroutine(ShockEffectCoroutine(duration));
    }

    private void StopShockEffect()
    {
        currentElementEffect = ElementType.None;
        currentCharge = 0;
        entityVFX.StopAllVFX();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVFX, transform.position, Quaternion.identity);
        entityHealth.ReduceHP(damage);
    }
    private IEnumerator ShockEffectCoroutine(float duration)
    {
        currentElementEffect = ElementType.Lightning;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopShockEffect();
    }

    public void ApplyBurnEffect(float duration, float fireDamage, int burnedTickPerSecond)
    {
        float fireResistance = entityStats.GetElementalReistance(ElementType.Fire);
        float reduceFireDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCoroutine(duration, reduceFireDamage, burnedTickPerSecond));
    }
    private IEnumerator BurnEffectCoroutine(float duration, float totalDamage, int tickPerSecond)
    {
        currentElementEffect = ElementType.Fire;
        entityVFX.PlayOnStatusVFX(duration, currentElementEffect);

        int tickCount = Mathf.RoundToInt(tickPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / tickPerSecond;

        for(int i = 0;i< tickCount; i++)
        {
            entityHealth.ReduceHP(damagePerTick, false);
            yield return new WaitForSeconds(tickInterval);
        }

        currentElementEffect = ElementType.None;
    }
    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalReistance(ElementType.Ice);
        float reduceDuration = duration * (1 - iceResistance);

        StartCoroutine(ChillEffectCoroutine(reduceDuration, slowMultiplier));
    }
    private IEnumerator ChillEffectCoroutine(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentElementEffect = ElementType.Ice;
        entityVFX.PlayOnStatusVFX(duration, currentElementEffect);

        yield return new WaitForSeconds(duration);

        currentElementEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if(element == ElementType.Lightning && currentElementEffect == ElementType.Lightning)
            return true;

        return currentElementEffect == ElementType.None;
    }
}
