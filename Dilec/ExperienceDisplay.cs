using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceDisplay : MonoBehaviour
{
    Experience XP;
    void Start()
    {
        XP = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }

    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", XP.GetPoints());
    }
}
