using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour ,ISaveable
{
    protected Player player;
    public event Action OnInventoryChange;
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    [Header("ITEM DATABASE")]
    [SerializeField] protected ItemList_DataSO itemDatabase;


    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }
    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item  == itemToUse);

        if (consumable == null) return;
        if (consumable.itemEffect.CanBeUsed(player) == false) return;
        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = StackableItem(itemToAdd) != null;
        return itemList.Count < maxInventorySize || hasStackable;
    }
    public Inventory_Item StackableItem(Inventory_Item itemToAdd)
    {
        return itemList.Find(item => item.itemData  == itemToAdd.itemData && item.CanAddStack());
    }

    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = StackableItem(itemToAdd);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);
        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);

        if(itemInInventory.stackSize>1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemToRemove);

            OnInventoryChange?.Invoke();
    }
    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        for (int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }
    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind );
    }

    public Inventory_Item FindSameItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item.itemData == itemToFind.itemData );
    }
    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();

    public virtual void LoadData(GameData data)
    {
        
    }

    public virtual void SaveData(ref GameData data)
    {
        
    }
}
