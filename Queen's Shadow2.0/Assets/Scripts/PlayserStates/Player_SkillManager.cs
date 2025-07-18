using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash {  get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }
    public Skill_DomainExpansion domainExpansion { get; private set; }

    public  Skill_Base[] allSkills { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();
        domainExpansion = GetComponentInChildren<Skill_DomainExpansion>();  

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldwonBy(float amout)
    {
        foreach (var skill in allSkills)
        {
            skill.ReduceCooldownBy(amout);
        }
    }

    public Skill_Base GetSkillByType(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return swordThrow;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.DomainExpansion: return domainExpansion;

            default: 
                Debug.Log($"Skill type not impolemented { skill}");
                return null;
        }
    }
}
