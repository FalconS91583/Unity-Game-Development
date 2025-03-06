using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private BoxCollider2D deathArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player is Death");
            GameManager.instance.isDead = true;
            GameManager.instance.Death();
        }
    }

}
