using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public ItemType slotType;

    private void OnValidate()
    {
        gameObject.name = "UI_EuipmentSlot - " +slotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        inventory.UnequipItem(itemInSlot);
    }
}
