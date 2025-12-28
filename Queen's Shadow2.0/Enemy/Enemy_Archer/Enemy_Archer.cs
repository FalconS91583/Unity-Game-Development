using UnityEngine;

public class Enemy_Archer : Enemy
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_ArcherBatlleState archerrBattleState { get; private set; }

    [Header("Archer specyfic")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowStartPoint;
    [SerializeField] private float arrowSpeed = 8;
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        archerrBattleState = new Enemy_ArcherBatlleState(this, stateMachine, "battle");
        battleState = archerrBattleState;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void SpecialAttack()
    {
        GameObject newArrow = Instantiate(arrowPrefab, arrowStartPoint.position, Quaternion.identity);
        newArrow.GetComponent<Enemy_ArcherArrow>().SetupArrow(arrowSpeed * facingDir, combat);
    }

    public void HandleCouter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

}
