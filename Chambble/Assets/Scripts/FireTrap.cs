using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float temperature = 10f; 
    [SerializeField] private float burnRate = 5f; 
    [SerializeField] private float maxTemperature = 20f; 
    private bool isBurning = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !isBurning)
        {
            StartCoroutine(BurnObject(rb, other.gameObject));
        }
        else if(other.tag == "Player")
        {
            GameManager.instance.isDead = true;
            GameManager.instance.Death();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator BurnObject(Rigidbody2D rb, GameObject obj)
    {
        isBurning = true;
        while (temperature < maxTemperature)
        {
            temperature += burnRate * Time.deltaTime;
            float progress = temperature / maxTemperature; 
            yield return null;
        }

        Destroy(obj);

        temperature = 10f;
        isBurning = false;
    }
}
