using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBufffs = new List<string>();
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuff(string source)
    {
        return activeBufffs.Contains(source) == false;  
    }

    public void ApplyBuff(BuffEffectData[] buffsToApply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToApply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBufffs.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifer(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifer(source);
        }

        inventory.TriggerUpdateUI();
        activeBufffs.Remove(source);
    }

}
