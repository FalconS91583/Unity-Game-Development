using System.Collections;
using UnityEngine;

// Convert statsus effect, to be activated when entity attack another liek 3 times, then apply effect liek burn
public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats stats;
    private Entity_Health health;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrigy Effect Details")]
    [SerializeField] private GameObject lightningStrikeVFX;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        health = GetComponent<Entity_Health>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentEffect = ElementType.None;
        entityVFX.StopAllVFX();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if(element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChilledEffect(effectData.chilldDuration, effectData.chillSlowDuration);

        if(element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration, effectData.burnDamage);
        
        if(element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyElectrifyEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }

    public void ApplyElectrifyEffect(float duration, float damage, float charge)
    {
        float lightninhRes = stats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightninhRes);
        currentCharge = currentCharge + finalCharge;

        if(currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
        }

        if(electrifyCo != null) 
            StopCoroutine(electrifyCo);

        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVFX.StopAllVFX();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVFX, transform.position, Quaternion.identity);
        health.ReduceHP(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);  

        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int ticksCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / ticksCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < ticksCount; i++)
        {
            health.ReduceHP(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceresistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1  - iceresistance);

        StartCoroutine(ChilledEffectCo(reducedDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntityBy(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }
}
