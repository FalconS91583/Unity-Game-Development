using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Refund Skill Item", fileName = "Item Effect Data - Refund")]
public class ItemEffect_RefundAllSkills : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
