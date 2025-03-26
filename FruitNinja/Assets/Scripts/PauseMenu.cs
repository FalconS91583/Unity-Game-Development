using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;//zmienna przechowuj¹ca czy gra jest zapauzowana 

    public GameObject pauseMenuUI;//zmienna do przechowywania kontroli nad UI 
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//if ktory po kliknieciu escape odpala b¹dz wy³¹cza skrypty zwiazane z pauza gry 
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pasue();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);//aktywacja puazy gry
        Time.timeScale = 1f;//mrozenie czasu
        GameIsPaused = false;//dzia³anie pauzy w grze 
    }

    void Pasue ()
    {
       pauseMenuUI.SetActive(true);//aktywacja puazy gry
        Time.timeScale = 0f;//mrozenie czasu
        GameIsPaused = true;//dzia³anie pauzy w grze 
    }
    public void LoadMenu()//Funkcja do wczytania menu z pauzy gry
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);//³adowanie sceny menu 
    }
    public void QuitGame()//Funckja wyjscia z gry w pauzie 
    {
        Application.Quit();
    }
}
