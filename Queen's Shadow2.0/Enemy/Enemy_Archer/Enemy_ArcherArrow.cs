using UnityEngine;

public class Enemy_ArcherArrow : MonoBehaviour, ICounterable
{
    [SerializeField] private LayerMask whatIsTarget;

    private Collider2D col;
    private Rigidbody2D rigid;
    private Entity_Combat combat;
    private Animator anim;

    public bool CanBeCountered => true;

    public void SetupArrow(float xVelocity, Entity_Combat combat)
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        this.combat = combat;
        rigid.linearVelocity = new Vector2 (xVelocity, 0);

        if (rigid.linearVelocity.x < 0)
            transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & whatIsTarget) != 0)
        {
            combat.PerformAttackOnTarget(other.transform);
            StuckIntoTarget(other.transform);
        }
    }

    private void StuckIntoTarget(Transform target)
    {
        rigid.linearVelocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        anim.enabled = false;

        transform.parent = target;

        Destroy(gameObject, 3);
    }

    public void HandleCouter()
    {
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x * -1, 0);
        transform.Rotate(0, 180, 0);

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        whatIsTarget |= (1 << enemyLayer);      
    }
}
