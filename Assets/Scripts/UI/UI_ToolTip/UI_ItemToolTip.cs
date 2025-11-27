using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform merchantInfo;
    [SerializeField] private Transform inventoryInfo;

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false, bool showMerchantInfo = false)
    {
        base.ShowToolTip(show, targetRect);

        merchantInfo.gameObject.SetActive(showMerchantInfo);
        inventoryInfo.gameObject.SetActive(!showMerchantInfo);

        int price = buyPrice? itemToShow.buyPrice: Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = (buyPrice ? "Giá mua: " : "Giá bán: ") + ($"{price} x {itemToShow.stackSize} = {totalPrice}g.");
        string singleStackPrice = (buyPrice ? "Giá mua: " : "Giá bán: ") + ($"{price}g.");

        itemPrice.text = itemToShow.stackSize > 1? fullStackPrice : singleStackPrice;
        itemType.text = itemToShow.GetItemTypeByName(itemToShow.itemData.itemType);
        itemInfo.text = itemToShow.GetItemInfo();

        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColorText(color, itemToShow.itemData.itemName);
    }
    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 100) return "green";   // Common
        if (rarity <= 300) return "blue";   // Uncommon
        if (rarity <= 600) return "purple";    // Rare
        if (rarity <= 850) return "yellow";  // Epic
        return "red";                     // Legendary
    }

}