using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [Range(1f, 50f)]
    [SerializeField] private int poolSize = 5;
    [Range(0.1f, 30f)]
    [SerializeField] private float spawnTime;

    private GameObject[] pool;

    private void Awake()
    {
        PopulatePool();
    }

    private void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for(int i =0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTime);  
        }
    }

    private void EnableObjectInPool()
    {
        for(int i =0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }
}
