using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data - ")]
public class Item_DataSO : ScriptableObject
{
    public string saveID { get; private set; }

    [Header("Merchant Details")]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("Drop Details")]
    [Range(0, 1000)] public int itemRarity = 100;
    [Range(0, 100)] public float DropChance;
    [Range(0, 100)] public float maxDropChance = 65f;

    [Header("Craft Details")]
    public Inventory_Item[] craftRecipe;

    [Header("Item Details")]
    public string itemName;
    public Sprite itemIcon;
    public Sprite inventoryIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item Effect")]
    public ItemEffectDataSO itemEffect;

#if UNITY_EDITOR
    private void OnValidate()
    {
        DropChance = GetDropChance();

        string path = UnityEditor.AssetDatabase.GetAssetPath(this);
        saveID = UnityEditor.AssetDatabase.AssetPathToGUID(path);
    }
#endif

    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxDropChance);
    }
}
