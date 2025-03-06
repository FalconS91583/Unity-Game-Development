using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float destroyDelay = 1f;
    private Animator anim;

    private bool isPopped = false;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPopped = true;

            Rigidbody2D chamsterRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (chamsterRb != null)
            {
                chamsterRb.linearVelocity = new Vector2(chamsterRb.linearVelocity.x, bounceForce);
                Debug.Log("Bubble hit!");
                PlayDestroyAnimation();
            }
        }
    }

    public virtual void PlayDestroyAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("Bloop"); 
        }

        Destroy(gameObject, destroyDelay);
    }
}
