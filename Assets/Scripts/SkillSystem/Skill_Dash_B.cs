using UnityEngine;

public class Skill_Dash_B : Skill_Base
{
    public void OnStartEffect()
    {
        if(Unlock(SkillUpgradeType.Dash_CloneOnStart_B1)||Unlock(SkillUpgradeType.Dash_CloneOnStartAndArrive_B11))
            CreateClone();

        if (Unlock(SkillUpgradeType.Dash_ShardOnStart_B2) || Unlock(SkillUpgradeType.Dash_ShardOnStartAndArrive_B21))
            CreateShard();
    }
    public void OnEndEffect()
    {
        if (Unlock(SkillUpgradeType.Dash_CloneOnStartAndArrive_B11))
            CreateClone();

        if (Unlock(SkillUpgradeType.Dash_ShardOnStartAndArrive_B21))
            CreateShard();
    }
    private void CreateShard()
    {
        skillManager.shard.CreateRawShard();
    }

    private void CreateClone()
    {
        skillManager.timeEcho.CreateTimeEcho();
    }
}
