using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private float startTime = 5f;

    private float timeLeft;
    private bool gameOver = false;

    public bool GameOver => gameOver;

    private void Start()
    {
        timeLeft = startTime;
    }

    private void Update()
    {
        DecreaseTime();
    }

    public void IncreateTime(float amount)
    {
        timeLeft += amount;
    }

    private void DecreaseTime()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        timeText.text = timeLeft.ToString("F2");

        if (timeLeft <= 0f)
        {
            PlayerGameOver();
        }
    }

    private void PlayerGameOver()
    {
        gameOver = true;
        player.enabled = false;
        gameOverText.SetActive(true);
        Time.timeScale = 0.1f;
    }
}
