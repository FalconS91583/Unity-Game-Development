using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//bibliteka do zarządzania scenami

public class MainMenu : MonoBehaviour
{
   public void StartGame()//załadowanie gry
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()//wyjście sz gry
    {
        Application.Quit();
    }
}
