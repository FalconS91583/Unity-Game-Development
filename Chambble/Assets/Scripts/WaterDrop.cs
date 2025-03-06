using UnityEngine;

public class WaterDropManager : MonoBehaviour
{
    public GameObject waterDropPrefab; 
    public float spawnInterval = 1f;  
    public float dropSpeed = 0.5f;    
    public float impactForce = 0.1f;  

    public float spawnHeight = 2f;    
    public float spawnRange = 1.5f;   

    private void Start()
    {
        InvokeRepeating("SpawnWaterDrop", 0f, spawnInterval);
    }

    private void SpawnWaterDrop()
    {
        float spawnX = transform.position.x + Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y + spawnHeight, 0f);

        GameObject waterDrop = Instantiate(waterDropPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = waterDrop.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = waterDrop.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = dropSpeed; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - impactForce);
            }
        }
    }
}
