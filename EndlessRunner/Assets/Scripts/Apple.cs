using UnityEngine;

public class Apple : Pickup
{
    [SerializeField] private float adjustChangeMoveSpeedAmout = 2f;

    private LevelGenerator levelGenerator;

    public void Init(LevelGenerator lg)
    {
        this.levelGenerator = lg;
    }
    protected override void OnPickUp()
    {
        levelGenerator.ChangeChunkmoveSpeed(adjustChangeMoveSpeedAmout);
    }
}
