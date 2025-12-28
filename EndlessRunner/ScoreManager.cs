using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoerText;

    int score = 0;

    public void IncreaseScore(int amount)
    {
        if (gameManager.GameOver) return;

        score += amount;
        scoerText.text = score.ToString();
    }
}
