using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoundType { Combat, Reward }

[System.Serializable]
public class EnemyWaveConfig
{
    public GameObject enemyPrefab; 
    public int count = 1;          
}

[System.Serializable]
public class RoundData
{
    public string roundName = "Round 1";
    public RoundType type;

    [Header("Combat Settings")]
    public List<EnemyWaveConfig> enemiesToSpawn; 

    [Header("Reward Settings")]
    public List<GameObject> possibleRewards; 
}
public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Configuration")]
    [SerializeField] private List<RoundData> allRounds;
    [SerializeField] private List<Transform> spawnPoints; 
    [SerializeField] private Transform rewardSpawnPoint; 
    private List<GameObject> spawnedRewards = new List<GameObject>();

    [Header("Spawn Area")]
    [SerializeField] private Vector2 minBounds; 
    [SerializeField] private Vector2 maxBounds; 

    [Header("Global Reward Pool")]
    [SerializeField] private List<GameObject> globalRewardPool; 
    [SerializeField]
    private int rewardsToSpawnCount = 2;

    private int currentRoundIndex = -1;
    private int activeEnemies = 0;
    private bool roundInProgress = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        RoundTrackerUI.Instance.SetupTracker(allRounds);
        StartNextRound();
    }

    public void StartNextRound()
    {
        currentRoundIndex++;
        RoundTrackerUI.Instance.UpdateCurrentRound(currentRoundIndex);
        if (currentRoundIndex >= allRounds.Count)
        {
            Debug.Log("KONIEC GRY - Wygra³eœ wszystkie rundy!");
            return;
        }

        RoundData currentRound = allRounds[currentRoundIndex];
        roundInProgress = true;

        Debug.Log($"Rozpoczynam rundê: {currentRound.roundName} [{currentRound.type}]");

        if (currentRound.type == RoundType.Combat)
        {
            StartCoroutine(SpawnEnemies(currentRound));
        }
        else if (currentRound.type == RoundType.Reward)
        {
            SpawnRewards(currentRound);
        }
    }


    private IEnumerator SpawnEnemies(RoundData round)
    {
        activeEnemies = 0;

        foreach (var wave in round.enemiesToSpawn)
        {
            for (int i = 0; i < wave.count; i++)
            {
                float randomX = Random.Range(minBounds.x, maxBounds.x);
                float randomY = Random.Range(minBounds.y, maxBounds.y);
                Vector3 randomPos = new Vector3(randomX, randomY, 0f);

                Instantiate(wave.enemyPrefab, randomPos, Quaternion.identity);

                activeEnemies++;

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void OnEnemyKilled()
    {
        if (!roundInProgress) return;

        activeEnemies--;

        if (activeEnemies <= 0)
        {
            Debug.Log("Wszyscy wrogowie pokonani! Przechodzê dalej...");
            roundInProgress = false;
           
            Invoke(nameof(StartNextRound), 2f);
        }
    }


    private void SpawnRewards(RoundData round)
    {
        spawnedRewards.Clear();

       
        List<GameObject> tempPool = new List<GameObject>(globalRewardPool);

        for (int i = 0; i < rewardsToSpawnCount; i++)
        {
            if (tempPool.Count == 0) break;

            int randomIndex = Random.Range(0, tempPool.Count);
            GameObject rewardToInstantiate = tempPool[randomIndex];


            Vector3 pos = rewardSpawnPoint.position + new Vector3(i * 3.0f - (rewardsToSpawnCount / 2f * 3.0f), 0, 0);

            GameObject reward = Instantiate(rewardToInstantiate, pos, Quaternion.identity);
            spawnedRewards.Add(reward);

            
            tempPool.RemoveAt(randomIndex);
        }
    }

    public void OnRewardChosen()
    {
        if (!roundInProgress) return;

        Debug.Log("Nagroda wybrana! Usuwam resztê i zaczynam now¹ rundê.");


        foreach (GameObject r in spawnedRewards)
        {
            if (r != null) Destroy(r);
        }
        spawnedRewards.Clear();

        roundInProgress = false;


        Invoke(nameof(StartNextRound), 1.5f);
    }
}