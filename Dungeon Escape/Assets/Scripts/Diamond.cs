using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int gems = 1;//zmienna trzymaj�ca warto�c diament�

    private void OnTriggerEnter2D(Collider2D other)//finckja do badania zderze� 
    {
        if (other.tag == "Player")//je�eli diament zderzy� si� z graczem 
        {
            Player player = other.GetComponent<Player>();//przypisanie do uchwytu skryptu gracza
            if( player != null)//sprawdzenie czy istnieje
            {
                player.AddGems(gems);//wywo�anie funkcji od dodawania gems�w od gracza z przekazaniem jej liczby              
                Destroy(this.gameObject);//niszczymy obiekt 
            }           
        }
    }
}
