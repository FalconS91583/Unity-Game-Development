using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;
    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;

    [Header("Heal Wisp Upgrade")]
    [SerializeField] private float damagePercentHealed = 0.4f;
    [SerializeField] private float cooldownReducenInSeconds;

    public float GetProcentageOfDamageHealed()
    {
        if (shouldBeWisp() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if(upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp)
            return 0;

        return cooldownReducenInSeconds;
    }

    public bool CanRemoveNegativeEffect()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;    
    }


    public bool shouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }
    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 0;

        return duplicateChance;
    }

    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack || upgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 1;

        if(upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttacks;

        return 0;
    }

    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreateTimeEcho();
        SetSkillOnCooldown();
    }

    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
