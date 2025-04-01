using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour {

    public GameObject gameOverCutscene;// zmienna przechowująca animacje, która się odpali w moemencie wypełnienia ifa w funckji niżej

    public Animator anim;//uchwyt do zarządzania animacja poprzez kod
    void OnTriggerEnter(Collider other)//funkcja działąjąca na zajście jakiegoś działąnia, w tym przypadku gdy ,, inny" przejdzie przez collider, który jest jako komponent CameraCone
    {
        if(other.tag == "Player") //jeżeli tak ,,other" to gracz wtedy
        {
            MeshRenderer render = GetComponent<MeshRenderer>();//handle aby mieć dostęp do meshRender komponentu w naszym cameraCone
            Color color = new Color(0.6f, .1f, .1f, .3f);//zmienna ustawiająca wymagany kolor poprzez RGB
            render.material.SetColor("_TintColor", color);//przypisanie do handle poprzez schodkowe dojście do wymaganego komponentu na kolor który ustwawiliśmy

            //render.material.color = Color.red;//tutaj idziemy schodkowo, nasza zmienna ma dostęp do meshrender więc najpierw przechodimy do materiały potem do koloru i ustawioamy nowy kolor
            anim.enabled = false; //zamrożenie animacji
            StartCoroutine(AlertRoutine());//odpalenie funkcji opóźnień oraz wywołanie po niej animacji
        }
    }

    IEnumerator AlertRoutine()//funkcja do opóźnień
    {
        yield return new WaitForSeconds(0.5f); //odczekanie pół sekundy
        gameOverCutscene.SetActive(true);//przechowana w zmiennej gameOverCutscene katscena się odpali jak sie spełni ten if
    }

}
