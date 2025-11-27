using TMPro;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;
    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent equipSlots;
    [SerializeField] private TextMeshProUGUI playerMoney;

    private void Update()
    {
        playerMoney.text = ($"{inventory.gold}G");
    }
    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        this.inventory.OnInventoryChange += UpdateSlotUI;
        this.merchant.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchantSlots)
            slot.SetupMerchantUI(merchant);
    }

    private void UpdateSlotUI()
    {
        if (inventory == null) return;
        inventorySlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
        equipSlots.UpdateEquipmentSlots(inventory.equipList);
    }
}
