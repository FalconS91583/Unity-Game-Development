using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float collisionCooldown = 1f;
    [SerializeField] private float adjustChangeMoveSpeedAmout = -1f;

    private float cooldownTimer = 0f;

    private LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (cooldownTimer < collisionCooldown) return;

        levelGenerator.ChangeChunkmoveSpeed(adjustChangeMoveSpeedAmout);
        anim.SetTrigger("Hit");
        cooldownTimer = 0f;
    }
}
