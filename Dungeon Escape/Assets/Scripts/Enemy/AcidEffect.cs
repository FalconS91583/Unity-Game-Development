using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    private void Start()//Wywy³uje siê przy odpaleniu gry
    {
        Destroy(this.gameObject, 3.0f);//zniszczenie obiektu tego po 5 sekundach je¿eli nie trafi w gracza
    }

    private void Update()//wywo³uje siê z ka¿da klatka w grze
    {
        transform.Translate(Vector3.right * 3 * Time.deltaTime);//ta linijka sprawia, ¿e nasz kwas strzelaj¹cy z paj¹ka bêdzie porusza³
        //siê w prawo z prêdkoœcia 3 jednostek pomno¿onê przez czas co daje 3 jednostki na sekunde

    }

    private void OnTriggerEnter2D(Collider2D other)//funkcja do badania czy w coœ siê walne³o 
    {
        if(other.tag == "Player")//jak czymœ jest gracz 
        {
            IDamageable hit = other.GetComponent<IDamageable>();//do zmiennej hit przypisujemy interfejs od damage
            if(hit != null)//i jezli gracz ma ten interfejs
            {
                hit.Damage();//wywy³ujemy metode od Damage
                Destroy(this.gameObject);//niszczymy obiekt je¿eli trafimy
            }

        }
    }

}
