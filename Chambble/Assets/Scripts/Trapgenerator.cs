using UnityEngine;

public class Trapgenerator : MonoBehaviour
{
    public GameObject trapPrefab1; 
    public GameObject trapPrefab2; 
    public Chamster chamster; 
    public float trapSpawnChance = 0.05f; 

    private void Start()
    {
        if (chamster != null)
        {
        }
    }

    private void Update()
    {
        if (chamster != null)
        {
            Vector3 playerPos = chamster.transform.position;

            if (Random.value < trapSpawnChance)
            {
                GameObject selectedTrap = (Random.value < 0.5f) ? trapPrefab1 : trapPrefab2;

                float randomX = Random.Range(2f, 10f); 
                Vector3 trapPosition = new Vector3(playerPos.x + randomX, -4.7f, playerPos.z); 

                Instantiate(selectedTrap, trapPosition, Quaternion.identity);
            }
        }
    }
}
