using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cmaeratrigger : MonoBehaviour {

    //zaczynamy zjebany kod gdzie tylko mam sie odwoływać co ten zjeb zrobił(chuj wie jak) i tylo sie do tego odwoływać :)
    public Transform myCameara; //zmienna która będzie przechowywała koordyantu kamery

    private void OnTriggerEnter(Collider other)//Funckaj która będzie coś robiła na trigerowaniu czegoś bierze collider do badania kolizji i other jako coś nie będące np Graczem (Other czyli czy coś się zderzyło z Toba(Player)) 
    {
        if(other.tag == "Player" ) {//gdy cos zderzy się z grzaczem to dzieje sie if
            Debug.Log("Trigger Activate");
            Camera.main.transform.position = myCameara.transform.position; //przpisanie pozycji dla poprawnej kamery
            Camera.main.transform.rotation = myCameara.transform.rotation;//przypisanie poprawnej rotacji kamery
        }
    }




}
