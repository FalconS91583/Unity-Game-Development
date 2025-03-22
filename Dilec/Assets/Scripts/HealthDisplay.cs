using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BramaBadura.Core;
using TMPro;
using System;

public class HealthDisplay : MonoBehaviour
{
    Health health;

    private void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxhealthPoints());
    }
}
