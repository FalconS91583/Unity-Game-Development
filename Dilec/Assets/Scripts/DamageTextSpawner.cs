using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] DamageText damagetextPrefab = null;

    public void Spawn(float damageAmount)
    {
        DamageText instance = Instantiate<DamageText>(damagetextPrefab, transform);
        instance.SetValue(damageAmount);
    }
}
