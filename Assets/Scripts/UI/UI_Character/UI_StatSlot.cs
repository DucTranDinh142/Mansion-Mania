using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats playerStats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if (statToUpdate == null && statSlotType != StatType.ElementalDamage) return;

        float value = 0;

        switch (statSlotType)
        {
            case StatType.Strength:
                value = playerStats.majorStat.strength.GetValue();
                break;

            case StatType.Agility:
                value = playerStats.majorStat.agility.GetValue();
                break;

            case StatType.Intelligence:
                value = playerStats.majorStat.intelligence.GetValue();
                break;

            case StatType.Vitality:
                value = playerStats.majorStat.vitality.GetValue();
                break;
            case StatType.MaxHP:
                value = playerStats.GetMaxHP();
                break;
            case StatType.HealthRegen:
                value = playerStats.resourceStat.healthRegen.GetValue();
                break;
            case StatType.Damage:
                value = playerStats.GetTotalDamage();
                break;
            case StatType.AttackSpeed:
                value = playerStats.offensiveStat.attackSpeed.GetValue() * 100;
                break;
            case StatType.CritPower:
                value = playerStats.GetTotalCritPower();
                break;
            case StatType.CritChance:
                value = playerStats.GetTotalCritChance();
                break;
            case StatType.ArmorReduction:
                value = playerStats.offensiveStat.armorReduction.GetValue();
                break;
            case StatType.IceDamage:
                value = playerStats.offensiveStat.iceDamage.GetValue();
                break;
            case StatType.FireDamage:
                value = playerStats.offensiveStat.fireDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offensiveStat.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element, 1);
                break;
            case StatType.Armor:
                value = playerStats.GetTotalArmor();
                break;
            case StatType.EvasionChance:
                value = playerStats.GetEvasion();
                break;
            case StatType.IceResistance:
                value = playerStats.GetElementalReistance(ElementType.Ice) * 100;
                break;
            case StatType.FireResistance:
                value = playerStats.GetElementalReistance(ElementType.Fire) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalReistance(ElementType.Lightning) * 100;
                break;
        }
        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
    }
    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Strength: return "STR";
            case StatType.Agility: return "AGI";
            case StatType.Intelligence: return "INT";
            case StatType.Vitality: return "VIT";
            case StatType.MaxHP: return "Máu tối đa";
            case StatType.HealthRegen: return "Hồi phục máu";
            case StatType.AttackSpeed: return "Tốc độ đánh";
            case StatType.Damage: return "Sát thương vật lý";
            case StatType.CritChance: return "Tỷ lệ chí mạng";
            case StatType.CritPower: return "Sát thương chí mạng";
            case StatType.ArmorReduction: return "Xuyên giáp";
            case StatType.FireDamage: return "Sát thương lửa";
            case StatType.IceDamage: return "Sát thương băng";
            case StatType.LightningDamage: return "Sát thương điện";
            case StatType.ElementalDamage: return "Sát thương nguyên tố";
            case StatType.Armor: return "Giáp";
            case StatType.EvasionChance: return "Tỷ lệ né tránh";
            case StatType.IceResistance: return "Kháng băng";
            case StatType.FireResistance: return "Kháng lửa";
            case StatType.LightningResistance: return "Kháng điện";
            default: return "Chỉ số không rõ";
        }
    }
    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.EvasionChance:
                return true;

            default:
                return false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statSlotType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

}
