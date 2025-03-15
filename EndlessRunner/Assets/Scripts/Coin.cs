using UnityEngine;

public class Coin : Pickup
{
    private ScoreManager scoreManager;
    [SerializeField] private int scoreAmount = 100;

    public void Init(ScoreManager SM)
    {
        this.scoreManager = SM;
    }
    protected override void OnPickUp()
    {
        scoreManager.IncreaseScore(scoreAmount);
    }
}
