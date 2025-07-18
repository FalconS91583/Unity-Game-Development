using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool needToCalculate = true;
    private float finalValue;
    public float GetValue()
    {
        if (needToCalculate)
        {
            finalValue = GetFinalValue();
            needToCalculate = false;   
        }

        return finalValue;
    }

    public void AddModifer(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source); //using constructor
        modifiers.Add(modToAdd);
        needToCalculate = true;
    }
    
    public void RemoveModifer(string source)
    {
        modifiers.RemoveAll(modifer => modifer.source == source); // works similar to foreach
        needToCalculate = true;
    }


    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue = finalValue + modifier.value;
        }

        return finalValue;
    }
    public void SetBaseValue(float value) => baseValue = value;
}

[System.Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
