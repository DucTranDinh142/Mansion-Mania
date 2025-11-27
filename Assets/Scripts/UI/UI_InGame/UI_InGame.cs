using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skillSlots;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Quick Item Slots")]
    [SerializeField] private float yOffsetQuickItemParent = 150;
    [SerializeField] private Transform quickItemOptionsParent;
    private UI_QuickItemSlotOption[] quickItemOptions;
    private UI_QuickItemSlot[] quickItemSlots;

    private void Start()
    {
        quickItemSlots = GetComponentsInChildren<UI_QuickItemSlot>();
        player = FindFirstObjectByType<Player>();
        player.health.OnHealthUpdate += UpdateHealthBar;

        inventory = player.inventory;
        inventory.OnInventoryChange += UpdateQuickSlotUI;
        inventory.OnQuickSlotUsed += PlayQuickSlotFeedback;
    }
    public void PlayQuickSlotFeedback(int slotNumber) => quickItemSlots[slotNumber].SimulateButtonFeedBack();
    public void UpdateQuickSlotUI()
    {
        Inventory_Item[] quickItems = inventory.quickItems;
        
        for(int i = 0; i < quickItems.Length; i++)
        {
            quickItemSlots[i].UpdateQuickSlotUI(quickItems[i]);

        }
    }
    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot,RectTransform targetRect)
    {
        if(quickItemOptions == null) 
            quickItemOptions = quickItemOptionsParent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);

        List<Inventory_Item> consumables = inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Consumable);

        for (int i =0; i< quickItemOptions.Length; i++)
        {
            if (i < consumables.Count)
            {
                quickItemOptions[i].gameObject.SetActive(true);
                quickItemOptions[i].SetupOption(quickItemSlot, consumables[i]);
            }
            else
                quickItemOptions[i].gameObject.SetActive(false);
        }
        quickItemOptionsParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
    }
    public void HideQuickItemOption() => quickItemOptionsParent.position = new Vector3(0,69420,0); 
    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        if (skillSlots == null)
            skillSlots = GetComponentsInChildren<UI_SkillSlot>(true);

        foreach (var slot in skillSlots)
        {
            if (slot.skillType == skillType)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        return null;
    }
    private void UpdateHealthBar()
    {
        float currentHealth = Mathf.RoundToInt(player.health.GetCurrentHealth());
        float maxHP = player.stats.GetMaxHP();
        float sizeDifference = Mathf.Abs(maxHP - healthRect.sizeDelta.x);

        if (sizeDifference > .1f)
            healthRect.sizeDelta = new Vector2(maxHP * 2f, healthRect.sizeDelta.y);

        healthText.text = currentHealth + "/" + maxHP;
        healthSlider.value = player.health.GetHPPercent();
    }

}
