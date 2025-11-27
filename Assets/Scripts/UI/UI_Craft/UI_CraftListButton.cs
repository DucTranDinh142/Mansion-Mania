using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemList_DataSO craftList;
    private UI_CraftSlot[] craftSlots;

    public void SetCraftSlots(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlots()
    {
        if (craftList == null) return;

        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < craftList.itemList.Length; i++)
        {
            Item_DataSO itemData = craftList.itemList[i];
            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemData);

        }
    }
}
