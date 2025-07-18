using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2f;
    [Header("Moving Shard")]
    [SerializeField] private float shardSpeed = 7;
    [Header("Multicast Shard")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    [Header("Teleport Shard")]
    [SerializeField] private float shardExistDuration = 10f;

    [Header("Teleport Healing Shard")]
    [SerializeField] private float savedHealthProcentage;


    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }
    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;


        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if(Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_TripleCast))
            HandleShardMulticast();

        if(Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if(Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            HandleShardHealthsRewind();
    }

    private void HandleShardHealthsRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthProcentage = playerHealth.GetHealthProcentage();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPrecent(savedHealthProcentage);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if(currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();

        player.TeleportPlayer(shardPosition);
    }

    private void HandleShardMulticast()
    {
        if (currentCharges <= 0)
            return;

        CreateShard();
        currentShard.MoveTowardClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while(currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }
        isRecharging =false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        float detonationTIme = GetDetotanteTime();

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            currentShard.OnExplode += ForceCooldwon;
    }

    public void CreateRawShard(Transform target = null, bool shardsCanMove= false)
    {
        bool canMove = shardsCanMove != false ? shardsCanMove :
            Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_TripleCast);


        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed, target);
    }

    public void CreateDomainShard(Transform target)
    {

    }

    public float GetDetotanteTime()
    {
        if(Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            return shardExistDuration;

        return detonateTime;
    }

    private void ForceCooldwon()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldwon;
        }
    }
}
