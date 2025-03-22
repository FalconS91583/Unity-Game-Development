using BramaBadura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private RectTransform foreground = null;
    [SerializeField] private Canvas rootCnavas = null;

    private void Update()
    {
        if(Mathf.Approximately(health.GetFraction(), 0) || Mathf.Approximately(health.GetFraction(), 1))
        {
            rootCnavas.enabled = false;
            return;
        }

        rootCnavas.enabled = true;

        foreground.localScale = new Vector3(health.GetFraction(), 1,1);
    }
}
