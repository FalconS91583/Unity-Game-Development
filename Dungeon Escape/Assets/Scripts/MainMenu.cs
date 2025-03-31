using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//bibliteka do zarz¹dzania scenami

public class MainMenu : MonoBehaviour
{
   public void StartGame()//za³adowanie gry
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()//wyjœcie sz gry
    {
        Application.Quit();
    }
}
