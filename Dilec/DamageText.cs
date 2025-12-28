using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText = null;
    public void DestroyText()
    {
        Destroy(gameObject);
    }

    public void SetValue(float amount)
    {
        damageText.text = amount.ToString();
    }
}
