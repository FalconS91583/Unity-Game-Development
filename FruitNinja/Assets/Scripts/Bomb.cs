using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)//funckja do badania zderzen biekt�w gry
    {
        if (other.CompareTag("Player"))//przy zderzeniu z ostrzem ( otagowane jako gracz) 
        {
            FindObjectOfType<GameMenager>().Explode();//wywo�uj� funckcje exploxe 
            
        }
    }
}
