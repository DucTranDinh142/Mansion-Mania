using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemList_DataSO dropData;
    [Header("Drop Restrictions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 4;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{

        //    DropItems();
        //}
    }

    public virtual void DropItems()
    {
        if(dropData == null) return;
        List<Item_DataSO> itemsToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i =0;  i < amountToDrop; i++)
        {
            CreateItemDrop(itemsToDrop[i]);
        }
    }

    protected void CreateItemDrop(Item_DataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    public List<Item_DataSO> RollDrops()
    {

        List<Item_DataSO> possibleDrops = new List<Item_DataSO>();
        List<Item_DataSO> finalDrops = new List<Item_DataSO>();
        float maxRarityAmount = this.maxRarityAmount;

        foreach(var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();
            if(Random.Range(0, 100) <= dropChance)
                possibleDrops.Add(item);
        }

        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        foreach(var item in possibleDrops)
        {
            if(maxRarityAmount > item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount -= item.itemRarity;
            }
        }

        return finalDrops;
    }
}
