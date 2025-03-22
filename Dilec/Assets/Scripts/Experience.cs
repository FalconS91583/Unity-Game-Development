using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float XP = 0;

    public event Action<float> OnExperienceGained;

    public void GainXP(float experience)
    {
        XP += experience;
        OnExperienceGained?.Invoke(XP);
    }

    public float GetPoints()
    {
        return XP;
    }
}
