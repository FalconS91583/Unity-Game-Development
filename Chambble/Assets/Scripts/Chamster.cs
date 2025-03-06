using UnityEngine;

public class Chamster : MonoBehaviour
{
    [SerializeField] private float xSpeed = 5f; 
    [SerializeField] private float maxHorizontalSpeed = 8f; 
    public Rigidbody2D rb;
    public int numberOfBubbles = 0;
    [SerializeField] private SafeBubble BubblePrefab;
    [SerializeField] private Needle needlePrefab;

    private int trigger;

    public float needleCooldownTime = 5f; 
    private float needleCooldownTimer = 0f; 

    private void Start()
    {
        trigger = Random.Range(2, 6);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateSafeBubble();
        }

        SendNeedle();
    }

    private void HandleMovement()
    {
        float xInput = Input.GetAxis("Horizontal");

        Vector2 newVelocity = rb.linearVelocity;
        newVelocity.x = Mathf.Clamp(xInput * xSpeed, -maxHorizontalSpeed, maxHorizontalSpeed);
        rb.linearVelocity = new Vector2(newVelocity.x, rb.linearVelocity.y);
    }

    private void CreateSafeBubble()
    {
        if (numberOfBubbles >= 3)
        {
            Instantiate(BubblePrefab, new Vector2(this.transform.position.x, this.transform.position.y - 2f), Quaternion.identity);
            numberOfBubbles -= 3; 
        }

        if (numberOfBubbles < trigger)
        {
            needleCooldownTimer = 0f; 
        }
    }

    private void SendNeedle()
    {
        needleCooldownTimer += Time.deltaTime;

        if (numberOfBubbles >= trigger && needleCooldownTimer >= needleCooldownTime)
        {
            Instantiate(needlePrefab, new Vector2(this.transform.position.x + 10f, this.transform.position.y), Quaternion.identity);

            needleCooldownTimer = 0f;

            trigger = Random.Range(2, 6);
        }
    }
}
