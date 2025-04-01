using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {

	public GameObject gameOverCutscene; //zmienna przechowująca cutscenke


	void OnTriggerEnter(Collider other) // funckja do wykrywania kolizju
	{
		if(other.tag == "Player")//jeżeli wykrywaną kolizją jest gracz wtedy odpal cutscenke
		{
			gameOverCutscene.SetActive(true);
		}
	}
}
		