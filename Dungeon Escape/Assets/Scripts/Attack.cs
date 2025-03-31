using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //zmienna pod tworzenie logiki aby damage wbija� si� raz z jednym ciachnieciem mieczem a nie, �e jeden klik to liczby jako 2 hity
    private bool _canDamage = true;//zmienna przchowuj�ca czy mo�emy zaatakowa�


    private void OnTriggerEnter2D(Collider2D other)//funkcja unity wykrywaj�ca w co si� waln�o, other jest czym� innym ni� to na czym jest skrypt
    {
        IDamageable hit = other.GetComponent<IDamageable>();//je�eli to co� w co walniemy posiada utworzony interfejst
        //to informacja o tym b�dzie zapisana w zmiennej hit
        if(hit != null )//je�li ta zmienna hit, nie jest null to
        {
            if(_canDamage == true)
            {
                hit.Damage();//wywo�uemy metode obra�e� z interfejsu
                _canDamage = false;
                StartCoroutine(DamageCooldown());
            } 
            
        }
    }
    IEnumerator DamageCooldown()//wprowadzenie op�nienia
    {
        yield return new WaitForSeconds(0.5f);//czekamy 0.5 sekundy
        _canDamage = true;//mo�na na nowo atakowa�
    }

}
