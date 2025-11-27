using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;


    [Header("Gold Info")]
    public int gold = 10000;

    public List<Inventory_EquipmentSlot> equipList;
    public Inventory_Storage storage { get; private set; }

    [Header("Quick Item Slots")]
    public Inventory_Item[] quickItems = new Inventory_Item[2];
    protected override void Awake()
    {
        base.Awake();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }
    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        TriggerUpdateUI();
    }
    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        if (itemToUse == null)
            return;

        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotNumber] = FindSameItem(itemToUse);
        }

        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);
    }
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHPPercent = player.health.GetHPPercent();
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddItemEffect(player);

        player.health.SetHPtoPercent(savedHPPercent);
        RemoveOneItem(itemToEquip);
    }
    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false) return;

        float savedHPPercent = player.health.GetHPPercent();

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;


        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHPtoPercent(savedHPPercent);
        AddItem(itemToUnequip);
    }

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.PercentHP = player.health.GetHPPercent();
        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;

                if (data.inventory.ContainsKey(saveId) == false)
                    data.inventory[saveId] = 0;

                data.inventory[saveId] += item.stackSize;
            }
        }
        foreach (var slot in equipList)
        {
            if (slot.HasItem())
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;
        foreach (var entry in data.inventory)
        {
            string saveID = entry.Key;
            int stackSize = entry.Value;

            Item_DataSO itemData = itemDatabase.GetItemData(saveID);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found");
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        foreach (var entry in data.equipedItems)
        {
            string saveId = entry.Key;
            ItemType loadedSlotType = entry.Value;

            Item_DataSO itemData = itemDatabase.GetItemData(saveId);
            Inventory_Item itemToLoad = new Inventory_Item(itemData);

            var slot = equipList.Find(slot =>
                slot.slotType == loadedSlotType &&
                slot.HasItem() == false);

            slot.equipedItem = itemToLoad;
            slot.equipedItem.AddModifiers(player.stats);
            slot.equipedItem.AddItemEffect(player);
        }
        if (data.PercentHP <= 0)
            player.health.SetHPtoPercent(1);
        else player.health.SetHPtoPercent(data.PercentHP);
        TriggerUpdateUI();
    }
}
