using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;//biblioteka do zarządania playable directorem z kodu(czyli zarządzanie cutscenkami)

public class GameManager : MonoBehaviour {

    //singleton Design Pattern
    private static GameManager _instance;//static tutaj powoduje, że ta zmienna jest zapisana w ogólnej pamięci czyli jest dopstepna teraz dla wszystkich klas i wgl wszystkiego
    //używać jej tylko gdy jest jedno wystapienie danego obiektu, czyli static moze być GameManager lub AudioManager, private jest po to, bo nie chemyżeby ktokolwiek mógł to zmodyfikować czy cos 
    //tearz to cały ten gameManager jest static czyli jest on dostępny  w pamięci ogólnej dla każdej kllasy itp
    public static GameManager _Instance//property, to jest coś takiego jak np w funkcjach podajemy transform.position = new Vector3( get , set) to za pomocą tego tworzymy włąsną taka metode, gdzie to my definiujemy co to jest co przyjmuje itp
    {
        get//jak mamy {} to oznacza zrób coś zanim weżmiesz wartość 
        {
            if(_instance == null)//jeżeli ten cały GameManager jest null to znaczy, że jest usuniety czy cokolwiek 
            {
                Debug.LogError("GameManager is null");//Error o braku Game manager
            }
            return _instance;//zwracamy dostęp do tego całego skryptu obiektu i wgl
        }
    }

    public bool HasCard { get; set; }//jest to automatyczne property, po protstu mały szybki zapis bez wiekszej logiki ze sobą
    public PlayableDirector introCutscene;//zmienna do cutscenki

    private void Awake()// na odpaleniu czy ładowaniu gry odrazu przypisajemy tego całego Managera do gry
    {
        _instance = this;   
    }

    public void Update()//funckja do badania co sie dzieje podczas jakiejś akcji w grze 
    {
        if (Input.GetKeyDown(KeyCode.S))//jeżeli naciśniety jest przycisk S 
        {
            introCutscene.time = 60.0f;//To skupijemy cutscenke do 60 sekundy czyli do końca
            AudioManager._Instance.PlayMusic();//Wywołanie z AudioManagera funkcji do grania muzyczki 
        }
    }

}
