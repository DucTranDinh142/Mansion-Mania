using Unity.VisualScripting;
using UnityEngine;

public class Skill_TimeEcho_A : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;
    [Header("Offensive Upgrade")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;
    [Header("Ultility Upgrade")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if(ShouldBeWisp() == false)
            return 0;
        return damagePercentHealed;
    }
    public float GetCooldownReduceInSeconds()
    {
        if(upgradeType != SkillUpgradeType.TimeEch_CoolDownWisp_A12)
            return 0;
        return cooldownReducedInSeconds;
    }
    public bool CanRemoveNegativeEffect() => upgradeType == SkillUpgradeType.TimeEcho_CleaseWisp_A11;
    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp_A1
            || upgradeType == SkillUpgradeType.TimeEcho_CleaseWisp_A11
            || upgradeType == SkillUpgradeType.TimeEch_CoolDownWisp_A12;
    }

    public float GetDuplicateChance()
    {
        if(upgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply_A22)
            return duplicateChance;

        return 0;
    }
    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack_A2 || upgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply_A22)
            return 1;
        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack_A21)
            return maxAttacks;

        return 0;
    }
    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false) return;
        base.TryUseSkill();
        CreateTimeEcho();
        SetSkillOnCoolDown();
    }
    public void CreateTimeEcho(Vector3? targetPostion = null)
    {
        Vector3 position = targetPostion ?? transform.position;
        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
