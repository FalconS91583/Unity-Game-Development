using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private Transform spawnPoint;

    private PlayerHealth player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(SpawnRobots());
    }

    IEnumerator SpawnRobots()
    {
        while(player)
        {
            Instantiate(robotPrefab, spawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
