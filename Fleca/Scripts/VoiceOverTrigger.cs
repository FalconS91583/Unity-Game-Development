using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour {

    public AudioClip ClipToPlay; // zmienna przechowująca dany dialog
    
    void OnTriggerEnter(Collider other) // funckja która wykonuje się kiedy wykryje jakąś kolizje czy coś, przejście przez dany punkt czy cos
    {
        if(other.tag == "Player") // jeżeli zderzjący się z tym jest gracz to wtedy wykonuje się if
        {
            AudioManager._Instance.PlayVoiceOver(ClipToPlay);//Przekazuejmy tutaj funkcje z audioMAnagera do odgrywania określonych audio
            //AudioSource.PlayClipAtPoint(ClipToPlay, Camera.main.transform.position);//audiosourde funkcja do wywołania odpalenia się dialogu dodatkowo z playclip który przyjmuje naszą zmienna do dialogów oraz pozycje, tutaj jest on pobierana przez funkcje Camera.main, która powoduje, że odnajdzie to main camere w grze i zagra tam dialog
        }
    }
}
