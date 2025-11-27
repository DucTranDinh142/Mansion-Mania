using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_EquipSlotParent equipmentSlotParent;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI soul;
    private UI ui;

    private void Update()
    {
        gold.text = ($"Vàng: {inventory.gold}G");
        soul.text = ($"Điểm linh hồn: {ui.skillTreeUI.skillPoints}");
    }
    private void Awake()
    {

        inventory = FindFirstObjectByType<Inventory_Player>();
        ui = FindFirstObjectByType<UI>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }
    private void OnEnable()
    {
        if (inventory == null) return;
        UpdateUI();
    }
    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
        equipmentSlotParent.UpdateEquipmentSlots(inventory.equipList);
    }
}
