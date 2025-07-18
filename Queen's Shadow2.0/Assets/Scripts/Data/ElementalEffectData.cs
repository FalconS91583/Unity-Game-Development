using UnityEngine;

[System.Serializable]
public class ElementalEffectData 
{
    public float chilldDuration;
    public float chillSlowDuration;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entityStats, DamageScaleData damageScale)
    {
        chilldDuration = damageScale.chillDuration;
        chillSlowDuration = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;

        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }
        
}
