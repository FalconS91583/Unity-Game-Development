using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class GameOver : MonoBehaviour
{
    public Text pointsText;//zmienna do przechowania tekstu na punkty
    public Text highScore;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " Points";

        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString() + " Highscore";
        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", score);
            highScore.text = score.ToString() + " Highscore";
        }
        
    }
    //³adowanie scen oraz automatcyczna funckja do wyjscia 
    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
