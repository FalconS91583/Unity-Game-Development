using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private float appleSpawnChance = 0.3f;
    [SerializeField] private float coinSpawnChance = 0.5f;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSeperationLenght = 2f;
    [SerializeField] private float[] lanes = { -2.5f, 0f, 2.5f };

    private LevelGenerator levelGenerator;
    private ScoreManager scoreManager;

    List<int> aveliableLanes = new List<int> { 0, 1, 2 };
    private void Start()
    {
        SpawnFence();
        SpawnApple();
        SpawnCoins();
    }

    public void Init(LevelGenerator lg, ScoreManager sm)
    {
        this.levelGenerator = lg;
        this.scoreManager = sm;
    }

    private void SpawnApple()
    {
        if (Random.value > appleSpawnChance) return;
        if (aveliableLanes.Count <= 0) return;
 
        int selectedLanes = SelectLanes();
        Vector3 spawnPosition = new Vector3(lanes[selectedLanes], transform.position.y, transform.position.z);
        Apple newApple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Apple>();
        newApple.Init(levelGenerator);
    }

    private void SpawnCoins()
    {
        if (Random.value > coinSpawnChance) return;
        if (aveliableLanes.Count <= 0) return;

        int selectedLanes = SelectLanes();
        int maxCoinsToSpawn = 6;
        int coinsToSpawn = Random.Range(1,maxCoinsToSpawn);
        float topOfChunkZ = transform.position.z + (coinSeperationLenght * 2);

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float spawnPositionZ = topOfChunkZ - (i * coinSeperationLenght);
            Vector3 spawnPosition = new Vector3(lanes[selectedLanes], transform.position.y, spawnPositionZ);
            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Coin>();
            newCoin.Init(scoreManager);
        }
    }

    private void SpawnFence()
    {
        int fancesToSpawn = Random.Range(0, lanes.Length);

        for (int i = 0; i < fancesToSpawn; i++)
        {
            if (aveliableLanes.Count <= 0) break;

            int selectedLanes = SelectLanes();
            Vector3 spawnPosition = new Vector3(lanes[selectedLanes], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }

    }

    private int SelectLanes()
    {
        int randomLane = Random.Range(0, aveliableLanes.Count);
        int selectedLanes = aveliableLanes[randomLane];
        aveliableLanes.RemoveAt(randomLane);
        return selectedLanes;
    }
}
