using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Ice Blast", fileName = "Item Effect Data - Ice Blast On Taking Damage")]
public class ItemEffect_ICeBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamge;
    [SerializeField] private LayerMask whatIsEnemy;
    [Space]
    [SerializeField] private float healthPrecentTrigger = 0.25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -999;
    [Header("VFX Objects")]
    [SerializeField] private GameObject iceBlastVFX;
    [SerializeField] private GameObject onHitVFX;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthProcentage() <= healthPrecentTrigger;

        if(noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVFX, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamge, ElementType.Ice, player.transform);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if(targetGotHit)
                player.vfx.CreateEffectOf(onHitVFX, target.transform); 
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.onTakingDamage += ExecuteEffect;
    }


    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.onTakingDamage -= ExecuteEffect;
        player = null;
    }
}
