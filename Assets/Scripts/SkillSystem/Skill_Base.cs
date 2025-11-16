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
        
    }
    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        scaleData = upgrade.scaleData;
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
    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;
}
