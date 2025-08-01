using UnityEngine;

public class Player_AnimationTriggers : Entity_AnimationsTriggers
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    private void ThrowSword() => player.skillManager.swordThrow.ThrowSword();
}
