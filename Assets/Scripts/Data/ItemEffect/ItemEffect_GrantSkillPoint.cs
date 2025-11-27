using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grant Skillpoint Effect", fileName = "Item Effect data - GrantSkillPoint - ")]
public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointToAdd);
    }
}
