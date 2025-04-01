using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//biblioteka do zarządania scenami
public class MainMenu : MonoBehaviour {

	//funckje do startu gry i wyjścia z niej
	public void StartGame()
	{
		SceneManager.LoadScene("LoadingScreen");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
