using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVFX;
    [SerializeField] private LayerMask whatIsGround;

    private bool shouldMoveToPlayer;

    private Transform playerTransfrom;

    private Skill_TimeEcho echoManager;

    private TrailRenderer wispTrail;
    private Entity_Health playerHealth;
    private SkillObject_Health echoHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;

    public int maxAttacks {  get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;

        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;

        maxAttacks = echoManager.GetMaxAttacks();

        playerTransfrom = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        skillManager = echoManager.skillManager;
        statusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
        FlipToTarget();

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);  

    }
    private void Update()
    {
        if(shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StoppHorizontalMovement();
        }
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransfrom.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransfrom.position) < 0.5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
        float healAmout = echoHealth.lastDamagetaken * echoManager.GetProcentageOfDamageHealed();
        playerHealth.IncreaseHealth(healAmout);

        float cooldownToReduce = echoManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldwonBy(cooldownToReduce);

        if(echoManager.CanRemoveNegativeEffect())
            statusHandler.RemoveAllNegativeEffects();
    }

    private void FlipToTarget()
    {
        Transform target = ClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if(targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0 ,0 ));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);

        if (echoManager.shouldBeWisp())
        {
            TurnIntoWisp();
        }
        else 
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StoppHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if(hit.collider != null) 
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
