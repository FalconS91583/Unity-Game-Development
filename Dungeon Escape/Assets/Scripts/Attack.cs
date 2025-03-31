using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //zmienna pod tworzenie logiki aby damage wbija³ siê raz z jednym ciachnieciem mieczem a nie, ¿e jeden klik to liczby jako 2 hity
    private bool _canDamage = true;//zmienna przchowuj¹ca czy mo¿emy zaatakowaæ


    private void OnTriggerEnter2D(Collider2D other)//funkcja unity wykrywaj¹ca w co siê walnê³o, other jest czymœ innym ni¿ to na czym jest skrypt
    {
        IDamageable hit = other.GetComponent<IDamageable>();//je¿eli to coœ w co walniemy posiada utworzony interfejst
        //to informacja o tym bêdzie zapisana w zmiennej hit
        if(hit != null )//je¿li ta zmienna hit, nie jest null to
        {
            if(_canDamage == true)
            {
                hit.Damage();//wywo³uemy metode obra¿eñ z interfejsu
                _canDamage = false;
                StartCoroutine(DamageCooldown());
            } 
            
        }
    }
    IEnumerator DamageCooldown()//wprowadzenie opóŸnienia
    {
        yield return new WaitForSeconds(0.5f);//czekamy 0.5 sekundy
        _canDamage = true;//mo¿na na nowo atakowaæ
    }

}
