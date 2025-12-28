using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BramaBadura.Stats;

public class LevelDisplay : MonoBehaviour
{
    BaseStats baseStats;
    void Start()
    {
        baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }

    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", baseStats.GetLevel());
    }
}
