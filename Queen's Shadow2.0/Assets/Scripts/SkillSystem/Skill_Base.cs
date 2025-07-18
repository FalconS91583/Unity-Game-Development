using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager {  get; private set; }
    public Player player;

    public DamageScaleData damageScaleData { get; private set; }

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    [SerializeField] private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed = lastTimeUsed - cooldown;
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(SkillDataSO skilldata)
    {
        UpgradeData upgrade = skilldata.upgradeData;

        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScale;

        player.ui.inGameUI.GetSkillSlot(skillType).SetupSkillSlot(skilldata);

        ResetCooldwon();
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    public SkillUpgradeType GetUpgrade() => upgradeType;
    public SkillType GetSkillType() => skillType;

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None) return false;

        if (OnCooldown())
        {
            Debug.LogWarning("On Cooldown");
            return false;
        }

        return true;
    }

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReduceCooldownBy(float cooldownreduction) => lastTimeUsed = lastTimeUsed + cooldownreduction;
    public void ResetCooldwon()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }
}
