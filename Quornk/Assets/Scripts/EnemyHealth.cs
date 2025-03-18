using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int starttingHealth = 3;
    [SerializeField] private int currentHealth;

    [SerializeField] private GameObject robotExplosion;

    private GameManager gameManager;

    private void Awake()
    {
        currentHealth = starttingHealth;
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.AdjustEnemies(1);
    }

    public void TakeDamage(int amout)
    {
        currentHealth -= amout;

        if (currentHealth <= 0)
        {
            gameManager.AdjustEnemies(-1);
           SelfDestruct();
        }
    }
    public void SelfDestruct()
    {
        Instantiate(robotExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
