using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText; 

    private float distanceTravelled; 
    private float startingPositionX; 
    private int highScore; 

    private void Start()
    {
        startingPositionX = transform.position.x;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Best: " + highScore + "m";
    }

    private void Update()
    {
        distanceTravelled = transform.position.x - startingPositionX;

        scoreText.text = "Score: " + Mathf.FloorToInt(distanceTravelled) + "m";

        if (distanceTravelled > highScore)
        {
            highScore = Mathf.FloorToInt(distanceTravelled);

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            highScoreText.text = "Best: " + highScore + "m";
        }
    }
}
