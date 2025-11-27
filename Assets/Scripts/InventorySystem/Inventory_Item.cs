using System;
using System.Text;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemID;
    public Item_DataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {  get; private set; }
    public ItemEffectDataSO itemEffect;

    public int buyPrice {  get; private set; }
    public float sellPrice { get; private set; }
    public Inventory_Item(Item_DataSO itemData)
    {
        this.itemData = itemData;

        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice*.69f;

        itemID = itemData.itemName + " - " + Guid.NewGuid();
    }
    private Equipment_DataSO EquipmentData()
    {
        if(itemData is Equipment_DataSO equipment)
            return equipment;

        return null;
    }
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemID);
        }
    }
    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemID);
        }
    }
    public void AddItemEffect(Player player) => itemEffect?.Subcribe(player);
    public void RemoveItemEffect() => itemEffect?.Unsubcribe();

    public bool CanAddStack() => stackSize < itemData.maxStackSize;

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
    public string GetItemInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (itemData.itemType == ItemType.Material)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Nguyên liệu chế tạo");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            return stringBuilder.ToString();
        }
            

        if (itemData.itemType == ItemType.Consumable)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine(itemEffect.effectDescription);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            return stringBuilder.ToString();
        }

        stringBuilder.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            stringBuilder.AppendLine("+ " + modValue + " " + modType);
        }

        if (itemEffect != null)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Nội tại vũ phí:");
            stringBuilder.AppendLine(itemEffect.effectDescription);
        }
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("");
        return stringBuilder.ToString();
    }

    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHP: return "Máu tối đa";
            case StatType.HealthRegen: return "Hồi phục máu";
            case StatType.Strength: return "STR";
            case StatType.Agility: return "AGI";
            case StatType.Intelligence: return "INT";
            case StatType.Vitality: return "VIT";
            case StatType.AttackSpeed: return "Tốc độ đánh";
            case StatType.Damage: return "Sát thương vật lý";
            case StatType.CritChance: return "Tỷ lệ chí mạng";
            case StatType.CritPower: return "Sát thương chí mạng";
            case StatType.ArmorReduction: return "Xuyên giáp";
            case StatType.FireDamage: return "Sát thương lửa";
            case StatType.IceDamage: return "Sát thương băng";
            case StatType.LightningDamage: return "Sát thương điện";
            case StatType.Armor: return "Giáp";
            case StatType.EvasionChance: return "Tỷ lệ né tránh";
            case StatType.IceResistance: return "Kháng băng";
            case StatType.FireResistance: return "Kháng lửa";
            case StatType.LightningResistance: return "Kháng điện";
            default: return "Chỉ số không rõ";
        }
    }
    public string GetItemTypeByName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Armor: return "Giáp trụ";
            case ItemType.Weapon: return "Vũ khí";
            case ItemType.Trinket: return "Trang sức";
            case ItemType.Material: return "Nguyên liệu";
            case ItemType.Consumable: return "Vật phẩm tiêu hao";
            default: return "Loại vật phẩm không rõ";
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

}