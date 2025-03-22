using BramaBadura.Combat;
using BramaBadura.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthDispaly : MonoBehaviour
{
    Fighter fighter;

    private void Awake()
    {
        fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
    }

    private void Update()
    {
        if (fighter.Gettarget() == null)
        {
            GetComponent<TextMeshProUGUI>().text = "N/A";
            return;
        }
        Health health = fighter.Gettarget();
        GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxhealthPoints());
    }
}
