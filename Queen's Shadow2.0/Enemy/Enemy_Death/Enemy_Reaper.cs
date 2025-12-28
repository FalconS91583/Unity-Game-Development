using System.Collections;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy_Reaper : Enemy, ICounterable
{
    public Enemy_ReaperAttackState reaperAttackState {  get; private set; }
    public Enemy_ReaperBattleState reaperBattleState { get; private set; }
    public Enemy_ReaperTeleportState reaperTeleportState { get; private set; }
    public Enemy_ReaperSpallCastState reaperSpellCastState { get; private set;} 

    [Header("Reaper Specific")]
    [SerializeField] public float maxBattleIdleTime = 5;

    [Header("Reaper Spellcast")]
    [SerializeField] private DamageScaleData spellDamageScale;
    [SerializeField] private GameObject spellCastPrefab;
    [SerializeField] private int amoutToCast = 6;
    [SerializeField] private float spellCastRate = 1.2f;
    [SerializeField] private float spellCastStateCooldown = 10;
    [SerializeField] private Vector2 playerOffsetPrediction;
    private float lastTimeCastedSpells = float.NegativeInfinity;
    public bool spellCastPerformed { get; private set; }
    private Player playerScript;
 
    [Header("Reaper teleport")]
    [SerializeField] private BoxCollider2D arenaBounds;
    [SerializeField] private float offsetCenterY = 1.72f;
    [SerializeField] private float chanceToTeleport = 0.25f;
    private float defaultTeleportChance;
    public bool teleportTrigger {  get; private set; }

    public bool CanBeCountered { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        reaperBattleState = new Enemy_ReaperBattleState(this, stateMachine, "battle");
        battleState = reaperBattleState;

        reaperAttackState = new Enemy_ReaperAttackState(this, stateMachine, "attack");

        reaperTeleportState = new Enemy_ReaperTeleportState(this, stateMachine, "teleport");

        reaperSpellCastState = new Enemy_ReaperSpallCastState(this, stateMachine, "spellCast");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        arenaBounds.transform.parent = null;
        defaultTeleportChance = chanceToTeleport;
    }

    public void HandleCouter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        StartCoroutine(CastSpellCo());
    }

    private IEnumerator CastSpellCo()
    {
        if (playerScript == null)
            playerScript = player.GetComponent<Player>();

        for (int i = 0; i < amoutToCast; i++)
        {
            bool playerMoving = playerScript.rb.linearVelocity.magnitude > 0;
            float xOffset = playerMoving ? playerOffsetPrediction.x * playerScript.facingDir : 0;
            Vector3 spellPosition = player.transform.position + new Vector3(xOffset, playerOffsetPrediction.y);

            Enemy_ReaperSpell spell =
                Instantiate(spellCastPrefab, spellPosition, Quaternion.identity).GetComponent<Enemy_ReaperSpell>();

            spell.SetupSpell(combat, spellDamageScale);

            yield return new WaitForSeconds(spellCastRate);
        }

        //anim.SetBool("spellCastPerformed", true);
        SetSpellCastPerformed(true);
    }

    public void SetSpellCastPerformed(bool spellCastStatus) => spellCastPerformed = spellCastStatus;
    public bool CanDoSpellCast() => Time.time > lastTimeCastedSpells + spellCastStateCooldown;
    public void SetSpellCastOnCooldown() => lastTimeCastedSpells = Time.time;
    public bool ShouldTeleport()
    {
        if(Random.value < chanceToTeleport)
        {
            chanceToTeleport = defaultTeleportChance;
            return true;
        }

        chanceToTeleport = chanceToTeleport + 0.05f;
        return false;    
    }

    public void SetTeleportTrigger(bool triggerStatus) => teleportTrigger = triggerStatus;

    public Vector3 FindTeleportPoint()
    {
        int maxAttempts = 10;
        float bossWidthColliderHalf = col.bounds.size.x / 2;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = 
                Random.Range(arenaBounds.bounds.min.x + bossWidthColliderHalf, arenaBounds.bounds.max.x - bossWidthColliderHalf);

            Vector2 raycastPoint = new Vector2(randomX, arenaBounds.bounds.max.y);
            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down, Mathf.Infinity, whatIsGround);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offsetCenterY);
        }


        return transform.position;
    }
}
