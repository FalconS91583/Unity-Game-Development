using UnityEngine;
using System.Collections.Generic;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPrecent = 0.8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Shard Casting Upgrade")]
    [SerializeField] private int shardToCast = 10;
    [SerializeField] private float shardCastingDomainSlowDown = 1;
    [SerializeField] private float shardCastingDomainDuration = 8;
    private float spellCastTimer;
    private float spellsPerSecond;

    [Header("Time Echo Casting Upgrade")]
    [SerializeField] private int echoToCast = 8;
    [SerializeField] private float echoCastDomainSlow = 1;
    [SerializeField] private float echoCastDomainDuration = 6;
    [SerializeField] private float healthToRestoreWithEcho = 0.05f;


    [Header("Domain Details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy> ();
    private Transform currentTarget;

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if(currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellsPerSecond;
            currentTarget  = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if(upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < .05f ? new Vector2(1,0) : new Vector2(-1,0);

            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if(upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true); 
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if(trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }
    public float GetDomainDuration()
    {
        if(upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownDomainDuration;
        else if(upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardCastingDomainDuration;
        else if(upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoCastDomainDuration;

        return 0;
    }

    public float GetSlowPrecent()
    {
        if(upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownPrecent;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardCastingDomainSlowDown;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoCastDomainSlow;

        return 0;
    }

    private int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardToCast;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoToCast;

        return 0;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam && upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }

    public void CreateDomain()
    {
        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void AddTarget(Enemy taretToAdd)
    {
        trappedTargets.Add(taretToAdd);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
        {
            enemy.StopSlowDown();
        }

        trappedTargets = new List<Enemy>();
    }
}
