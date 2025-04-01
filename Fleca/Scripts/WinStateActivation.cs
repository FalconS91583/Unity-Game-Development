using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinStateActivation : MonoBehaviour {

    public GameObject winCutscene;//zmienna przechowująca cutscenke
    void OnTriggerEnter(Collider other)//funkcja do badania kolizji
    {
        if(other.tag == "Player")//jeżeli wykryje kolizje z graczem 
        {
            if(GameManager._Instance.HasCard == true) //oraz jeżeli gracz ma karte(Sprawdzane przez GameManagera)
            {
                winCutscene.SetActive(true);//odpalamy cutscenke
            }
            else
            {
                Debug.Log("Grab the Key Card");
            }
        }
    } 

}
