using UnityEngine;

[System.Serializable]
public class AttackData : MonoBehaviour
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entityStats, DamageScaleData scaleData) 
    {
        physicalDamage = entityStats.GetPsyhicalDamage(out isCrit, scaleData.physical);
        elementalDamage = entityStats.GetElementalDamage(out element, scaleData.elemental);

        effectData = new ElementalEffectData(entityStats, scaleData);
    }
    
}
