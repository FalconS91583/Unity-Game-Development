using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeGameManager : MonoBehaviour
{
    private int score;
    public Text scoreText;
    public Text bestScoreText;
    public GameObject restartButton;
    public GameObject exitButton;
    private int bestScore;

    //funkcja od zerowania punktów na start gry
    private void Start()
    {
        score = 0;
        UpdateScoreText();
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreText();
        restartButton.SetActive(false);
        exitButton.SetActive(false);

    }
    //funkcja od podnoszenia wyniku gry 
    public void IncreaseScore()
    {
        score += 10;
        UpdateScoreText();
        UpdateBestScore();
    }
    //funkcja od aktualizowania wyniku gry
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + bestScore;
    }
    //reset wyniku 
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    //zakoñczenie gry
    public void EndGame()
    {
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }
    //reset gry
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //wyjœcie z gry
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    //aktualizacja najlepszego wyniku 
    private void UpdateBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
    }


}
