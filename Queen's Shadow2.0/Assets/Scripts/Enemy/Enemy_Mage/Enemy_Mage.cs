using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Mage : Enemy, ICounterable   
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_MageQuickRetreatState quickRetreatState { get; private set; }
    public Enemy_MageBattleState mageBattleState { get; private set; }
    public Enemy_MageSpellCastState spellCastState { get; private set; }

    [Header("Mage specific")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private int amountToCast = 3;
    [SerializeField] private float spellCastCooldown = 0.3f;
    [SerializeField] private Transform spellStartPosition;
    public bool spellCastPerformed { get; private set; }    
    [Space]
    public float retreatCooldown = 5;
    public float retreatMaxDistance = 8;
    public float retreatSpeed = 15;
    [SerializeField] private Transform behindCollisionCheck;
    [SerializeField] private bool hasRecoveryAnimation = true;
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        quickRetreatState = new Enemy_MageQuickRetreatState(this, stateMachine, "battle");
        mageBattleState = new Enemy_MageBattleState(this, stateMachine, "battle");
        battleState = mageBattleState;

        spellCastState = new Enemy_MageSpellCastState(this, stateMachine, "spellCast");


        anim.SetBool("hasRecoveryAnimation", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void SetSpellCastPerformed(bool performed) => spellCastPerformed = performed;

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        StartCoroutine(CastSpellCo());
    }

    private IEnumerator CastSpellCo()
    {
        for (int i = 0; i < amountToCast; i++)
        {
            Enemy_MageProjectile projectile =
                Instantiate(spellPrefab, spellStartPosition.position, Quaternion.identity).GetComponent < Enemy_MageProjectile>();

            projectile.SetupProjectile(player.transform, combat);

            yield return new WaitForSeconds(spellCastCooldown);
        }

        //anim.SetBool("spellCastPerformed", true);
        SetSpellCastPerformed(true);
    }

    public void HandleCouter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public bool CantMoveBackwards()
    {
        bool detectedWall = Physics2D.Raycast(behindCollisionCheck.position, Vector2.right * -facingDir, 1.5f, whatIsGround);
        bool noGround = Physics2D.Raycast(behindCollisionCheck.position, Vector2.down, 1.5f , whatIsGround) == false;

        return noGround || detectedWall;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(behindCollisionCheck.position, 
            new Vector3(behindCollisionCheck.position.x + (1.5f * -facingDir), behindCollisionCheck.position.y));
        Gizmos.DrawLine(behindCollisionCheck.position, 
            new Vector3(behindCollisionCheck.position.x, behindCollisionCheck.position.y - 1.5f));
    }
}
