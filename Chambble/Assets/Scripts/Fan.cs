using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour
{
    [SerializeField] private float pushForce = 5f; 
    [SerializeField] private float pushDuration = 0.1f;
    private bool isPushing = false;


    private void Start()
    {
        pushForce = Random.Range(5, 8);
        pushDuration = Random.Range(0.1f,0.2f);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !isPushing && other.tag != "Player")
        {
            StartCoroutine(PushObject(rb));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isPushing = false;
    }

    private IEnumerator PushObject(Rigidbody2D rb)
    {
        isPushing = true;
        while (isPushing)
        {
            rb.AddForce(Vector2.up * pushForce, ForceMode2D.Force);
            yield return new WaitForSeconds(pushDuration);
        }
    }
}
