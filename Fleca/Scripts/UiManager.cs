using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Biblioteka do zarządzania scenami 
public class UiManager : MonoBehaviour
{

	//singleton, bo to manager raz na cała gre
	//Nie opisany kod jest taki sam jak w GameManager;

	private static UiManager _instance;
	public static UiManager _Instance

	{
		get
		{
			if (_Instance == null)
			{
				Debug.LogError("UiiManager is null");
			}

			return _Instance;

		}
	}

	private void Awake()
	{

		_instance = this;
	}

	public void Restart()//Funkcja do restartu gry
	{
		SceneManager.LoadScene("Main");//załadowanie jeszcze raz sceny gry
	}

	public void Quit ()//funkcja do wyjścia z gry
	{
		Application.Quit();//Funkcja z biblioteki do po prostu wyjścia z gry
	}
	//te dwie funkcje pamiętać trzeba aby przypisać w Unity tam gdzie mamy buttony to akcje na onclick i wybrać ten skrypt i wtedy z rozwinajego muny juz wybrać daną funkcje


}
