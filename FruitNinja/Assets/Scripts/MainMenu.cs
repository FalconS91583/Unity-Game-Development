using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//potrzebne do zarzadzania scenami

public class MainMenu : MonoBehaviour
{
    public void PlayGame()//Funkcja do prze³¹czenia siê do sceny z gr¹ 
    {
        SceneManager.LoadScene(1);//za³adowanie pierwszej sceny 
        //sceny jak tablica numeruje sie od 0 wiec 0 to menu 1 to gra ustalane w File -> build settings
    }

    public void QuitGame()//Funckcja wyjscia z gry
    {
        Application.Quit();
    }
}
