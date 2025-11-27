using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewSlot[] craftRecipeSlots;

    [Header("Item Preview Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;
        craftRecipeSlots = GetComponentsInChildren<UI_CraftPreviewSlot>();
        foreach(var slot in craftRecipeSlots)
            slot.gameObject.SetActive(false);
    } 

    public void ConfirmCraft()
    {
        if(itemToCraft == null)
        {
            buttonText.text = "Chọn đồ để chế tạo.";
            return;
        }

        if (storage.CanCraftItem(itemToCraft))
            storage.CraftItem(itemToCraft);

        UpdateCraftPreviewSlots();
    }

    public void UpdateCraftPreview(Item_DataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);
        itemIcon.sprite = itemData.inventoryIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();

        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftRecipeSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftRecipeSlots[i].gameObject.SetActive(true);
            craftRecipeSlots[i].SetupPreviewSlot(requiredItem.itemData, availableAmount, requiredAmount);
        }
    }
}
