using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grand Skill Point", fileName = "Item Effect Data - Gran Skill Point")]
public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
    
}
