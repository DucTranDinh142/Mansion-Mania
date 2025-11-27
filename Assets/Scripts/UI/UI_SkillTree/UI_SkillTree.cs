using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour, ISaveable
{
    public int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }

    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPoint();
    }
    public void UpdateSkillPoint()
    {
        skillPointsText.text = skillPoints.ToString();
    }
    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach (UI_TreeNode node in allTreeNodes)
            node.UnlockDefaultSkill();
    }
    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
            node.Refund();
    }
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPoint();
    }
    public void AddSkillPoints(int points)
    {
        skillPoints += points;
        UpdateSkillPoint();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints;

        foreach (var node in allTreeNodes)
        {
            if (data.skillTreeUI.TryGetValue(node.skillData.skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUpgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node =>
                    node.skillData.upgradeData.upgradeType == upgradeType);

                if (upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }
        }


    }

    public void SaveData(ref GameData data)
    {
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();
        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.skillName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }
        foreach (var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}
