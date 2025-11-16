using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill description")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;
}

[System.Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public ScaleData scaleData;
}
