using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    public GameObject bubblePrefab; 
    public GameObject carrotPrefab; 
    public GameObject trapPrefab1; 
    public GameObject trapPrefab2; 
    public GameObject upperObstacle1; 
    public GameObject upperObstacle2; 
    public Chamster chamster; 
    public float radius = 5f; 
    public int maxBubbles = 2; 
    public float carrotSpawnChance = 0.005f; 
    public float trapSpawnChance = 0.005f; 
    public float upperObstacleSpawnChance = 0.003f; 

    

    private void Start()
    {
      
        if (chamster != null)
        {
           
        }
    }

    private void Update()
    {
        GenerateOnLevel();
        DifHandler();
    }

    private void DifHandler()
    {
        float timer = Time.time;

        if(timer <= 15)
        {
            Easy();
        }
        else if (timer > 15 && timer <= 40)
        {
            Medium();
        }
        else
        {
            Hard();
        }
    }

    private void Easy()
    {
        Debug.Log("Easy state");

        radius = 10f;
        maxBubbles = 4;
        carrotSpawnChance = 0.005f;
        trapSpawnChance = 0.00004f;
        upperObstacleSpawnChance = 0.00004f;
        chamster.needleCooldownTime = 15f; 
}

    private void Medium()
    {
        Debug.Log("Medium state");
        radius = 15f;
        maxBubbles = 2;
        carrotSpawnChance = 0.0005f;
        trapSpawnChance = 0.0004f;
        upperObstacleSpawnChance = 0.0004f;
        chamster.needleCooldownTime = 7f;
    }
    private void Hard()
    {
        Debug.Log("Hard state");
        radius = 20f;
        maxBubbles = 1;
        carrotSpawnChance = 0.000005f;
        trapSpawnChance = 0.004f;
        upperObstacleSpawnChance = 0.004f;
        chamster.needleCooldownTime = 3f;
    }

    private void GenerateOnLevel()
    {
        if (chamster != null)
        {
            Vector3 playerPos = chamster.transform.position;
            Collider2D[] nearbyBubbles = Physics2D.OverlapCircleAll(playerPos, radius);

            
            int bubbleCount = 0;
            foreach (var bubble in nearbyBubbles)
            {
                if (bubble.CompareTag("Bubble")) 
                {
                    bubbleCount++;
                }
            }

            
            if (bubbleCount < maxBubbles)
            {
                float randomX = Random.Range(2f, 10f); 
                float randomY = Random.Range(-5f, 5f); 
                Vector3 bubblePosition = new Vector3(playerPos.x + randomX, randomY, playerPos.z);

                bool isPositionOccupied = false;
                foreach (var bubble in nearbyBubbles)
                {
                    if (bubble.transform.position == bubblePosition)
                    {
                        isPositionOccupied = true;
                        break;
                    }
                }

                if (!isPositionOccupied) 
                {
                    Instantiate(bubblePrefab, bubblePosition, Quaternion.identity);
                }
            }

            
            if (Random.value < carrotSpawnChance)
            {
                float randomX = Random.Range(2f, 10f); 
                float randomY = Random.Range(-5f, 5f); 
                Vector3 carrotPosition = new Vector3(playerPos.x + randomX, randomY, playerPos.z);

                bool isPositionOccupied = false;
                foreach (var bubble in nearbyBubbles)
                {
                    if (bubble.transform.position == carrotPosition)
                    {
                        isPositionOccupied = true;
                        break;
                    }
                }

                if (!isPositionOccupied) 
                {
                    Instantiate(carrotPrefab, carrotPosition, Quaternion.identity);
                }
            }

           
            if (Random.value < trapSpawnChance)
            {
                GameObject selectedTrap = (Random.value < 0.5f) ? trapPrefab1 : trapPrefab2;
                float randomX = Random.Range(2f, 10f); 
                Vector3 trapPosition = new Vector3(playerPos.x + randomX, -4.7f, playerPos.z);

                bool isPositionOccupied = false;
                foreach (var bubble in nearbyBubbles)
                {
                    if (bubble.transform.position == trapPosition)
                    {
                        isPositionOccupied = true;
                        break;
                    }
                }

                if (!isPositionOccupied) 
                {
                    Instantiate(selectedTrap, trapPosition, Quaternion.identity);
                }
            }

            
            if (Random.value < upperObstacleSpawnChance)
            {
                GameObject selectedObstacle = (Random.value < 0.5f) ? upperObstacle1 : upperObstacle2;
                float randomX = Random.Range(2f, 10f); 
                Vector3 obstaclePosition = new Vector3(playerPos.x + randomX, 7.8f, playerPos.z);

                bool isPositionOccupied = false;
                foreach (var bubble in nearbyBubbles)
                {
                    if (bubble.transform.position == obstaclePosition)
                    {
                        isPositionOccupied = true;
                        break;
                    }
                }

                if (!isPositionOccupied) 
                {
                    Instantiate(selectedObstacle, obstaclePosition, Quaternion.identity);
                }
            }
        }
    }
}
