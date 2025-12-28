using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data -")]
public class SkillDataSO : ScriptableObject
{
    [Header("Skill Description")]
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
    public DamageScaleData damageScale;
}
