using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//bibliteka do zarz�dzania scenami

public class MainMenu : MonoBehaviour
{
   public void StartGame()//za�adowanie gry
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()//wyj�cie sz gry
    {
        Application.Quit();
    }
}
