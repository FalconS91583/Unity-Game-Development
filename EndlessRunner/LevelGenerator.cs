using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private CameraController camera;
    [SerializeField] private GameObject[] Chunkprefab;
    [SerializeField] private GameObject checkpointCHunkPrefab;
    [SerializeField] private int startingChunksAmout = 12;
    [SerializeField] private int checkpointCHunkInterval = 8;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private ScoreManager scoreManager;

    private float chunkLenght = 10f;

    [SerializeField] private float moveSpeed = 8f;
    private float minMoveSpeed = 2f;
    private float maxMoveSpeed = 12f;

    private float minGravityZ = -22f;
    private float maxGravityZ = -8f;

    private List<GameObject> chunks = new List<GameObject>();

    private int chunkSpawned = 0;

    private void Start()
    {
        GeneratePath();
    }

    private void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkmoveSpeed(float speedAmout)
    {
        float newMoveSpeed = moveSpeed + speedAmout;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if(newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmout;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            camera.ChangeCameraFOV(speedAmout);
        }
    }

    private void GeneratePath()
    {
        for (int i = 0; i < startingChunksAmout; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);

        GameObject chunkToSpawn = ChooseChunkToSpawn();
        GameObject newChunkGO = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);

        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager);

        chunkSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;
        if (chunkSpawned % checkpointCHunkInterval == 0 && chunkSpawned != 0)
        {
            chunkToSpawn = checkpointCHunkPrefab;
        }
        else
        {
            chunkToSpawn = Chunkprefab[Random.Range(0, Chunkprefab.Length)];
        }

        return chunkToSpawn;
    }

    private float CalculateZ()
    {
        float spawnPositionZ;

        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count -1].transform.position.z + chunkLenght;
        }

        return spawnPositionZ;
    }

    private void MoveChunks() 
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if(chunk.transform.position.z <= Camera.main.transform.position.z - chunkLenght)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }
}
