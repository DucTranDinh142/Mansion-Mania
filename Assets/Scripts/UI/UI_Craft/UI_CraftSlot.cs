using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private Item_DataSO itemToCraft;
    [SerializeField] private UI_CraftPreview craftPreview;

    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;

    public void SetupButton(Item_DataSO craftData)
    {
        this.itemToCraft = craftData;
        craftItemIcon.sprite = craftData.inventoryIcon;
        craftItemName.text = craftData.itemName;
    }
    public void UpdateCraftPreview() => craftPreview.UpdateCraftPreview(itemToCraft);
}
