using UnityEngine;

public class Enemy_Slime : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_SlimeDeadState slimeDeadState { get; set; }

    [Header("Slime specific")]
    [SerializeField] private bool hasRecoveryAnimation = true;

    [SerializeField] private GameObject childSlimePrefab;
    [SerializeField] private int amountOfSlimesToMake = 2;
    [SerializeField] private Vector2 velocityOnCreation;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        slimeDeadState = new Enemy_SlimeDeadState(this, stateMachine, "idle");

        anim.SetBool("hasRecoveryAnimation", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void HandleCouter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public override void EntityDeath()
    {
        stateMachine.ChangeState(slimeDeadState);
    }

    public void CreateSlime()
    {
        if (childSlimePrefab == null)
            return;

        for (int i = 0; i < amountOfSlimesToMake; i++)
        {
            GameObject newSlime = Instantiate(childSlimePrefab, transform.position, Quaternion.identity);
            Enemy_Slime slimeScript = newSlime.GetComponent<Enemy_Slime>();

            slimeScript.stats.SetupStatsWithPenlty(stats.resources, stats.offense, stats.defense, .6f, 1.2f);
            slimeScript.ApplyrespawnValocity();
            slimeScript.StartBattleStateCheck(player);
        }
    }

    public void ApplyrespawnValocity()
    {
        Vector2 velocity = new Vector2(stunnedVelocity.x * Random.Range(-1f, 1f), stunnedVelocity.y * Random.Range(1f, 2f));
        SetVelocity(velocity.x, velocity.y);
    }

    public void StartBattleStateCheck(Transform player)
    {
        TryEnterBattleState(player);
        InvokeRepeating(nameof(ReEnterBattleState), 0, 0.3f);
    }

    public void ReEnterBattleState()
    {
        if(stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            CancelInvoke(nameof(ReEnterBattleState));
            return;
        }
        
        stateMachine.ChangeState(battleState);
    }
}
