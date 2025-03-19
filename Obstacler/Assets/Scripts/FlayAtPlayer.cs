using UnityEngine;

public class FlayAtPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 playerPos;
    [SerializeField] private float speed = 10f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void Start()
    {
        playerPos  = player.transform.position;

    }
    void Update()
    {
        MoveToPlayer();
        DestroyWhenReached();
    }

    public void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed* Time.deltaTime);
    }
    private void DestroyWhenReached()
    {

        if (transform.position == playerPos)
        {
            Destroy(this.gameObject);
        }
    }
}
