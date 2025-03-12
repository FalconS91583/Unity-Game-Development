using UnityEngine;
//klasa odpowiedzialna za jedzenie
public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public SnakeGameManager gameManager;
    private void Start()
    {
        RandomizePosition();
    }
    //funkcja do losowania pozycji jedzenia w zakresie planszy 
    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }
    //funkcja odpowiadaj¹ca za zderzenie gracza(Snake) z jedzeniem oraz dodawaniem punków po zdobyciu jedzenia
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameManager = FindObjectOfType<SnakeGameManager>();

        if (other.CompareTag("Player"))
        {
            RandomizePosition();
            gameManager.IncreaseScore();
        }
    }

}
