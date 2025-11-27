using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player inventory { get; private set; }
    public List<Inventory_Item> materialStash;

    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeMaterials(itemToCraft);
        inventory.AddItem(itemToCraft);
    }
    public  bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && inventory.CanAddItem(itemToCraft);
    }
    private void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requireItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requireItem.stackSize;

            amountToConsume -= ConsumeMaterialsAmount(inventory.itemList, requireItem);
            if (amountToConsume > 0)
                amountToConsume -= ConsumeMaterialsAmount(itemList, requireItem);

            if (amountToConsume > 0)
                amountToConsume -= ConsumeMaterialsAmount(materialStash, requireItem);
        }
    }
    private int ConsumeMaterialsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumeAmount = 0;

        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            var item = itemList[i];

            if (item.itemData != neededItem.itemData)
                continue;

            int removeAmount = (int)MathF.Min(item.stackSize, amountNeeded - consumeAmount);

            item.stackSize -= removeAmount;
            consumeAmount += removeAmount;

            if (item.stackSize <= 0)
                itemList.RemoveAt(i);

            if (consumeAmount >= amountNeeded)
                break;
        }

        return consumeAmount;
    }


    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requireMaterial in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailableAmountOf(requireMaterial.itemData) < requireMaterial.stackSize) return false;
        }
        return true;
    }

    public int GetAvailableAmountOf(Item_DataSO requireItem)
    {
        int amount = 0;

        foreach (var item in inventory.itemList)
        {
            if (item.itemData == requireItem)
            {
                amount += item.stackSize;
            }
        }
        foreach (var item in itemList)
        {
            if (item.itemData == requireItem)
            {
                amount += item.stackSize;
            }
        }
        foreach (var item in materialStash)
        {
            if (item.itemData == requireItem)
            {
                amount += item.stackSize;
            }
        }
        return amount;
    }
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);
        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(newItemToAdd);
        }

        TriggerUpdateUI();
        materialStash = materialStash.OrderBy(item => item.itemData.itemRarity).ToList();
    }
    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        return materialStash.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());


    }
    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItem(item))
            {
                var itemToTransfer = new Inventory_Item(item.itemData);
                inventory.RemoveOneItem(item);
                AddItem(itemToTransfer);
            }
        }


        TriggerUpdateUI();
    }
    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (inventory.CanAddItem(item))
            {
                var itemToTransfer = new Inventory_Item(item.itemData);
                RemoveOneItem(item);
                inventory.AddItem(itemToTransfer);
            }
        }

        TriggerUpdateUI();
    }

    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);

        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;

                if (data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;

                data.storageItems[saveId] += item.stackSize;
            }
        }

        data.storageMaterials.Clear();

        foreach (var item in materialStash)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;

                if (data.storageMaterials.ContainsKey(saveId) == false)
                    data.storageMaterials[saveId] = 0;

                data.storageMaterials[saveId] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
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

        foreach (var entry in data.storageMaterials)
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
                AddMaterialToStash(itemToLoad);
            }
        }
    }
}
