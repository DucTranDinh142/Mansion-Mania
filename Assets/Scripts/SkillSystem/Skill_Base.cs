using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager {  get; private set; }
    public Player player {  get; private set; }
    public ScaleData scaleData { get; private set; }

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;
    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed -= cooldown; 
        scaleData = new ScaleData();
    }
    public virtual void TryUseSkill()
    {
        player.entitySFX.Skilling();
    }
    public void SetSkillUpgrade(Skill_DataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        scaleData = upgrade.scaleData;


        player.ui.inGameUI.GetSkillSlot(skillType).SetupSkillSlot(skillData);
        ResetCooldown();
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None) return false;
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;   
        }

        return true;
    }

    protected bool Unlock(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    public SkillUpgradeType GetUpgrade() => upgradeType;

    public SkillType GetSkillType() => skillType;

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCoolDown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }
}
