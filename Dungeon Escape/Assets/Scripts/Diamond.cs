using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int gems = 1;//zmienna trzymaj¹ca wartoœc diamentó

    private void OnTriggerEnter2D(Collider2D other)//finckja do badania zderzeñ 
    {
        if (other.tag == "Player")//je¿eli diament zderzy³ siê z graczem 
        {
            Player player = other.GetComponent<Player>();//przypisanie do uchwytu skryptu gracza
            if( player != null)//sprawdzenie czy istnieje
            {
                player.AddGems(gems);//wywo³anie funkcji od dodawania gemsów od gracza z przekazaniem jej liczby              
                Destroy(this.gameObject);//niszczymy obiekt 
            }           
        }
    }
}
