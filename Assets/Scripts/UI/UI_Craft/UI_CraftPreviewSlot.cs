using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviewSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    public void SetupPreviewSlot(Item_DataSO itemData, int availableAmount, int requireAmount)
    {
        materialIcon.sprite = itemData.inventoryIcon;
        materialNameValue.text = itemData.itemName+": "+availableAmount+"/"+requireAmount;
    }
}
