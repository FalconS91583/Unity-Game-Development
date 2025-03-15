using UnityEngine;

public class CHeckpoint : MonoBehaviour
{
    [SerializeField] private float checkpointTimeExtension = 5f;
    [SerializeField] private float obstacleDecreateTimeAmount = .2f;

    private GameManager gameManager;
    private ObstacleSpawner obstacleSpawner;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameManager.IncreateTime(checkpointTimeExtension);
            obstacleSpawner.DecreaseObstacleSpawnTime(obstacleDecreateTimeAmount);
        }
    }
}
