using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

	public Transform target;//zmienna do przechowywania celu gdzie to ma sie zawsze patrzec 
	public Transform startCamera;//zmienna do przechowywania pozycji startowej kamery
	private void Start()//funckaj która na starcie przeypisuje poprawnie ustawienie i rotacje kamery przy odpaleiu gry 
	{
		transform.position = startCamera.position; 
		transform.rotation = startCamera.rotation;
	}
	void Update () {
		transform.LookAt(target);//target nasz gracz i robimy zeby patrzał na niego
	}
}
