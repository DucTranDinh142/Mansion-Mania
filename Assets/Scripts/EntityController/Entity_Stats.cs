using System.Data;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetUpSO defaultStatSetup;

    public Stat_ResourceStat resourceStat;
    public Stat_DefensiveStat defensiveStat;
    public Stat_OffensiveStat offensiveStat;
    public Stat_MajorStat majorStat;

    public float GetMaxHP()
    {
        float baseMaxHP = resourceStat.maxHP.GetValue();
        float bonusMaxHP = majorStat.vitality.GetValue() * 5;
        float FinalMaxHP = baseMaxHP + bonusMaxHP;

        return FinalMaxHP;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offensiveStat.damage.GetValue();
        float bonusDamage = majorStat.strength.GetValue();
        float totalDamage = baseDamage + bonusDamage;

        float baseCritChance = offensiveStat.critChance.GetValue();
        float bonusCritChance = majorStat.agility.GetValue() * .3f;
        float totalCritChance = baseCritChance + bonusCritChance;
        isCrit = Random.Range(0, 100) < totalCritChance;

        float baseCritPower = offensiveStat.critPower.GetValue();
        float bonusCritPower = majorStat.strength.GetValue() * .5f;
        float totalCritPower = (baseCritPower + bonusCritPower) / 100;

        float finalCalculatedDamage = isCrit ? totalDamage * totalCritPower : totalDamage;

        return finalCalculatedDamage * scaleFactor;
    }
    public float GetArmorRedution()
    {
        float finalReduction = offensiveStat.armorReduction.GetValue() / 100;

        return finalReduction;
    }
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defensiveStat.armor.GetValue();
        float bonusArmor = majorStat.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (100 + effectiveArmor);
        float mitigationCap = .80f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }
    public float GetEvasion()
    {
        float baseEvasionChance = defensiveStat.evasionChance.GetValue();
        float bonusEvasionChance = majorStat.agility.GetValue() * .5f;

        float totalEvasionChance = baseEvasionChance + bonusEvasionChance;
        float evasionChanceCap = 60;

        float finalEvasionChance = Mathf.Clamp(totalEvasionChance, 0, evasionChanceCap);

        return finalEvasionChance;
    }
    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDamage = offensiveStat.fireDamage.GetValue();
        float iceDamage = offensiveStat.iceDamage.GetValue();
        float lightningDamage = offensiveStat.lightningDamage.GetValue();
        float bonusElementalDamage = majorStat.intelligence.GetValue();

        float highestElementalDamage = iceDamage;
        element = ElementType.Ice;

        if (fireDamage > highestElementalDamage)
        {
            highestElementalDamage = fireDamage;
            element = ElementType.Fire;
        }

        if (lightningDamage > highestElementalDamage)
        {
            highestElementalDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestElementalDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }
        float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * .5f;
        float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * .5f;
        float bonusLightning = (element == ElementType.Lightning) ? 0 : lightningDamage * .5f;

        float WeakerElementalDamageBonus = bonusIce + bonusFire + bonusLightning;

        float finalElementalDamage = highestElementalDamage + bonusElementalDamage + WeakerElementalDamageBonus;

        return finalElementalDamage * scaleFactor;
    }
    public float GetElementalReistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = majorStat.intelligence.GetValue() * 0.5f;
        switch (element)
        {
            case ElementType.Ice:
                baseResistance = defensiveStat.iceResistance.GetValue(); break;
            case ElementType.Fire:
                baseResistance = defensiveStat.fireResistance.GetValue(); break;
            case ElementType.Lightning:
                baseResistance = defensiveStat.lightingResistance.GetValue(); break;
        }

        float elementResistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalElementResistance = Mathf.Clamp(elementResistance, 0, resistanceCap) / 100;

        return finalElementResistance;
    }
    public Stat GetStatByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Vitality: return majorStat.vitality;
            case StatType.Strength: return majorStat.strength;
            case StatType.Agility: return majorStat.agility;
            case StatType.Intelligence: return majorStat.intelligence;

            case StatType.MaxHP: return resourceStat.maxHP;
            case StatType.HealthRegen: return resourceStat.healthRegen;

            case StatType.Damage: return offensiveStat.damage;
            case StatType.AttackSpeed: return offensiveStat.attackSpeed;
            case StatType.CritPower: return offensiveStat.critPower;
            case StatType.CritChance: return offensiveStat.critChance;
            case StatType.ArmorReduction: return offensiveStat.armorReduction;
            case StatType.IceDamage: return offensiveStat.iceDamage;
            case StatType.FireDamage: return offensiveStat.fireDamage;
            case StatType.LightingDamage: return offensiveStat.lightningDamage;

            case StatType.Armor: return defensiveStat.armor;
            case StatType.EvasionChance: return defensiveStat.evasionChance;
            case StatType.FireResistance: return defensiveStat.fireResistance;
            case StatType.IceResistance: return defensiveStat.iceResistance;
            case StatType.LightingResistance: return defensiveStat.lightingResistance;

            default:
                Debug.LogWarning($"StatType {statType} not implemented yet");
                return null;


        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultSetUp()
    {
        if (defaultStatSetup == null) return;

        majorStat.vitality.SetBaseValue(defaultStatSetup.vitality);
        majorStat.strength.SetBaseValue(defaultStatSetup.strength);
        majorStat.agility.SetBaseValue(defaultStatSetup.agility);
        majorStat.intelligence.SetBaseValue(defaultStatSetup.intelligence);

        resourceStat.maxHP.SetBaseValue(defaultStatSetup.maxHealth);
        resourceStat.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        offensiveStat.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offensiveStat.damage.SetBaseValue(defaultStatSetup.damage);
        offensiveStat.critChance.SetBaseValue(defaultStatSetup.critChance);
        offensiveStat.critPower.SetBaseValue(defaultStatSetup.critPower);
        offensiveStat.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offensiveStat.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offensiveStat.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offensiveStat.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defensiveStat.armor.SetBaseValue(defaultStatSetup.armor);
        defensiveStat.evasionChance.SetBaseValue(defaultStatSetup.evasion);

        defensiveStat.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
        defensiveStat.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
        defensiveStat.lightingResistance.SetBaseValue(defaultStatSetup.lightningResistance);

    }
}
