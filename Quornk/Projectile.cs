using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private GameObject projectileHitFX;
    private Rigidbody rb;

    public int damage = 2;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(int damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage);

        Instantiate(projectileHitFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
