using System;
using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour, IDamagable
{
    public event Action onTakingDamage;
    public event Action OnHealthUpdate;

    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats stats;
    private Entity_DropManager dropManager;

    private bool miniHealthBarActive;
    [SerializeField] protected float currentHp;
    public bool isDead {  get; private set; }
    protected bool canTakeDamage = true;

    [Header("Health Regen")]
    [SerializeField] private float regenInterval;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamagetaken {  get; private set; }

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 onDamgeKnockback = new Vector2(1.5f, 2.5f);
    [Header("On Heavy Damage Knockback")]
    [Range(0f, 1f)]
    [SerializeField] private float heavyDamageThreshold = 0.3f;
    [SerializeField] private float heavyKnockDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
        dropManager = GetComponent<Entity_DropManager>();

        healthBar = GetComponentInChildren<Slider>();

        SetupHealth();

    }

    protected virtual void Start()
    {
        
    }

    private void SetupHealth()
    {

        if (stats == null)
            return;

        currentHp = stats.GetMaxHealth();
        OnHealthUpdate += UpdatehealthBar;

        UpdatehealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false) return false;

        if (AttackEvaded())
            return false;

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats != null ? stats.GetArmorMitigation(armorReduction) : 0;
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = stats != null ? stats.GetElementalResistance(element) : 0;
        float elementDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);

        ReduceHP(physicalDamageTaken + elementDamageTaken);

        lastDamagetaken = physicalDamageTaken + elementDamageTaken;

        onTakingDamage?.Invoke();

        return true;
    }

    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        float duration = CalculateDuration(finalDamage);
        Vector2 knocbkack = CalcutateKnockback(finalDamage, damageDealer);

        entity?.ReciveKnockback(knocbkack, duration);
    }

    private bool AttackEvaded()
    {
        if (stats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < stats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        float regenAmout = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmout);
    }

    public void IncreaseHealth(float healAmout)
    {
        if (isDead)
            return;

        float newHealth = currentHp + healAmout;
        float maxHealth = stats.GetMaxHealth();

        currentHp = Mathf.Min(newHealth, maxHealth);
        OnHealthUpdate?.Invoke();
    }

    public void ReduceHP(float damage)
    {
        currentHp -= damage;
        entityVFX?.PlayOnDamageVFX();
        OnHealthUpdate?.Invoke();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdatehealthBar()
    {
        if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
        {
            return;
        }
        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    public void EnableHealthBar(bool enable) => healthBar?.transform.parent.gameObject.SetActive(enable);

    public float GetCurrentHealth() => currentHp;

    protected virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
        dropManager?.DropItems();
    }

    public float GetHealthProcentage() => currentHp / stats.GetMaxHealth();

    public void SetHealthToPrecent(float precent)
    {
        currentHp = stats.GetMaxHealth() * Mathf.Clamp01(precent);
        OnHealthUpdate?.Invoke();
    }

    private Vector2 CalcutateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamgeKnockback;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        if (stats == null)
            return false;
        else
            return damage / stats.GetMaxHealth() > heavyDamageThreshold;
    }

}
