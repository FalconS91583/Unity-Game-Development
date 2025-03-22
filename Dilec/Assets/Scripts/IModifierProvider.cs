using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierProvider 
{
    IEnumerable<float> GetModifier(Stat stat);
    IEnumerable<float> GetProcentageModifiers(Stat stat);
}
