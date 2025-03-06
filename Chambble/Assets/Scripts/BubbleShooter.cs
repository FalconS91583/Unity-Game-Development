using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab; 
    public float detectionRadius = 5f; 
    public float minForce = 5f; 
    public float maxForce = 10f; 
    public float shootInterval = 1f; 
    public int numberOfBubbles = 5; 

    private Transform player; 
    private bool isPlayerInRange = false; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                InvokeRepeating("ShootBubbles", 0f, shootInterval);
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                CancelInvoke("ShootBubbles");
            }
        }
    }

    private void ShootBubbles()
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = bubble.AddComponent<Rigidbody2D>();
            }

            Vector2 direction = (player.position - bubble.transform.position).normalized;

            float force = Random.Range(minForce, maxForce);
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
