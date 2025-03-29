using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private GameObject _PausemenuPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver==true)
        {
            SceneManager.LoadScene(1);//mozna tez po nazwie sceny
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            _PausemenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        _PausemenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
