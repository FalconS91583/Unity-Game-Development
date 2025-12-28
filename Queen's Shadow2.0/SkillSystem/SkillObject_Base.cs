using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVFX;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physicalDamage = attackData.physicalDamage;
            float elementalDamge = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamge, element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVFX, target.transform.position, Quaternion.identity);
            }

            usedElement = element;

        }
    }

    protected Transform ClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }


    protected virtual void OnDrawGizmos()
    {
        if(targetCheck != null) 
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
