using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Amnestics Effect", fileName = "Item Effect data - Amnestics - ")]
public class ItemEffect_Amnestics : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
       UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
