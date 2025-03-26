using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//potrzebne do zarzadzania scenami

public class MainMenu : MonoBehaviour
{
    public void PlayGame()//Funkcja do prze��czenia si� do sceny z gr� 
    {
        SceneManager.LoadScene(1);//za�adowanie pierwszej sceny 
        //sceny jak tablica numeruje sie od 0 wiec 0 to menu 1 to gra ustalane w File -> build settings
    }

    public void QuitGame()//Funckcja wyjscia z gry
    {
        Application.Quit();
    }
}
