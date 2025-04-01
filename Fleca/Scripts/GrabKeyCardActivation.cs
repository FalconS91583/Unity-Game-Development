using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabKeyCardActivation : MonoBehaviour {

    public GameObject sleepingGuardCutscene;//zmienna jako objekt gry do przypisania cutscenki w edytorze

    private void OnTriggerEnter(Collider other)//klasycnie funkcja która się aktywuje przy kolizji z czymś
    {
        if(other.tag == "Player")//jeżeli czymś jest gracz 
        {
            sleepingGuardCutscene.SetActive(true);//to animacja sie odpala
            GameManager._Instance.HasCard = true;//W taki sposób w nbardzo szybki dostajemy się do klasy Menagera i u stawiamy interesującą nas klase na true w celu powiedzenia, ze gracz zebrał karte
            //Jest to szybszy sposób niż zrobienie uchwytu a potem znalezenie odpowiedniej klasy, oszczędność i te sprawy
        }
    } 

}
