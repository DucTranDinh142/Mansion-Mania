using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash_B dash { get; private set; }
    public Skill_Shard_C shard { get; private set; }
    public Skill_SwordThrow_D swordThrow { get; private set; }

    public Skill_TimeEcho_A timeEcho { get; private set; }
    public Skill_Ultimate_E ultimate { get; private set; }

    private Skill_Base[] allskills;

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash_B>();
        shard = GetComponentInChildren<Skill_Shard_C>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow_D>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho_A>();
        ultimate = GetComponentInChildren<Skill_Ultimate_E>();

        allskills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allskills)
        {
            skill.ReduceCooldownBy(amount);
        }
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash_B: return dash;
            case SkillType.Shard_C: return shard;
            case SkillType.SwordThrow_D: return swordThrow;
            case SkillType.TimeEcho_A: return timeEcho;
            case SkillType.Ultimate_E: return ultimate;

            default:
                Debug.Log($"There is no {type} skill");
                return null;
        }
    }
}
