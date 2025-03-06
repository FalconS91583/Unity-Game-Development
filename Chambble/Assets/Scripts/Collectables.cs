using UnityEngine;

public class Collectables : Bubble
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Chamster chamster = collision.gameObject.GetComponent<Chamster>();
            chamster.numberOfBubbles++;
            Destroy(this.gameObject);
        }

    }
}
