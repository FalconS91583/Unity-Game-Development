using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] private float speed = 5f; 
    [SerializeField] private float lifetime = 10f; 
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.instance.isDead = true;
            GameManager.instance.Death();
        }
    }
}
