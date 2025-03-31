using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    private void Start()//Wywy�uje si� przy odpaleniu gry
    {
        Destroy(this.gameObject, 3.0f);//zniszczenie obiektu tego po 5 sekundach je�eli nie trafi w gracza
    }

    private void Update()//wywo�uje si� z ka�da klatka w grze
    {
        transform.Translate(Vector3.right * 3 * Time.deltaTime);//ta linijka sprawia, �e nasz kwas strzelaj�cy z paj�ka b�dzie porusza�
        //si� w prawo z pr�dko�cia 3 jednostek pomno�on� przez czas co daje 3 jednostki na sekunde

    }

    private void OnTriggerEnter2D(Collider2D other)//funkcja do badania czy w co� si� walne�o 
    {
        if(other.tag == "Player")//jak czym� jest gracz 
        {
            IDamageable hit = other.GetComponent<IDamageable>();//do zmiennej hit przypisujemy interfejs od damage
            if(hit != null)//i jezli gracz ma ten interfejs
            {
                hit.Damage();//wywy�ujemy metode od Damage
                Destroy(this.gameObject);//niszczymy obiekt je�eli trafimy
            }

        }
    }

}
