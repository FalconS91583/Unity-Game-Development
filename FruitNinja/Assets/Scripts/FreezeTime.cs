using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTime : MonoBehaviour
{
    public GameObject freezeText;
    private void OnTriggerEnter(Collider other)//funckja do badania zderzen biekt�w gry
    {
        if (other.CompareTag("Player"))//przy zderzeniu z ostrzem ( otagowane jako gracz) 
        {
            FindObjectOfType<GameMenager>().Freeze();//wywo�uj� funckcje exploxe 

        }
    }
}
