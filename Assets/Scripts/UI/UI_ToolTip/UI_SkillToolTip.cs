using System;
using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantInfoHex;
    [SerializeField] private Color testColor;
    [SerializeField] private string lockedSkillText = "*Bạn đã chọn nhánh kỹ năng khác rồi, nhánh kỹ năng này đã bị khóa vĩnh viễn*";

    private Coroutine textEffectCoroutine;
    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
        Console.OutputEncoding = Encoding.UTF8;
    }
    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);

    }
    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSO skillData, UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);

        if (show == false) return;

        skillName.text = skillData.skillName;

        skillDescription.text = skillData.description +"\n\n\nHồi chiêu: "+ skillData.upgradeData.cooldown+" giây.";

        if(node == null)
        {
            skillRequirements.text = "";
            return;
        }

        string skillLockedText = GetColorText(importantInfoHex, lockedSkillText);
        string requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNode);

        skillRequirements.text = requirements;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Yêu cầu mở khóa kỹ năng:");
        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metConditionHex : notMetConditionHex;
        string costText = $"*{skillCost} điểm linh hồn.";
        string finalCostText = GetColorText(costColor, costText);

        stringBuilder.AppendLine(finalCostText);

        foreach (var node in neededNodes)
        {
            if(node == null) continue;
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            string nodeText = $"*{node.skillData.skillName}";
            string finalNodeText = GetColorText(nodeColor, nodeText);

            stringBuilder.AppendLine(finalNodeText);
        }

        if (conflictNodes.Length <= 0)
            return stringBuilder.ToString();

        stringBuilder.AppendLine(); // spacing
        stringBuilder.AppendLine(GetColorText(importantInfoHex, "Lưu ý: Các kỹ năng sau sẽ bị khóa sau khi mở khóa kỹ năng này:"));

        foreach (var node in conflictNodes)
        {
            if(node == null) continue;
            string nodeText = $"*{node.skillData.skillName}";
            string finalNodeText = GetColorText(importantInfoHex, nodeText);
            stringBuilder.AppendLine(finalNodeText);
        }

        return stringBuilder.ToString();
    }


    public void LockedSkillEffect()
    {
        StopLockedSkillEffect();

        textEffectCoroutine = StartCoroutine(TextBlinkEffectCoroutine(skillRequirements, .15f, 3));
    }
    public void StopLockedSkillEffect()
    {
        if (textEffectCoroutine != null)
            StopCoroutine(textEffectCoroutine);
    }
    private IEnumerator TextBlinkEffectCoroutine(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColorText(notMetConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColorText(importantInfoHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }

    }
}
