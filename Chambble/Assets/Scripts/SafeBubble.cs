using UnityEngine;

public class SafeBubble : Bubble
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void PlayDestroyAnimation()
    {
        base.PlayDestroyAnimation();
    }
}
