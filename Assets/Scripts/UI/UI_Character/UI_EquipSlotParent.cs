using UnityEngine;
using System.Collections.Generic;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipmentSlot[] equipmentSlots;

    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equipList)
    {
        if(equipmentSlots == null)
            equipmentSlots = GetComponentsInChildren<UI_EquipmentSlot>();

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            var playerEquipSlot = equipList[i];

            if (playerEquipSlot.HasItem() == false)
                equipmentSlots[i].UpdateSlot(null);
            else
                equipmentSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }
}
