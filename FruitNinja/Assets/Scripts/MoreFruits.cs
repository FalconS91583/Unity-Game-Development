using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreFruits : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)//funckja do badania zderzen biektów gry
    {
        if (other.CompareTag("Player"))//przy zderzeniu z ostrzem ( otagowane jako gracz) 
        {
            FindObjectOfType<GameMenager>().Volcano();//wywo³ujê funckcje exploxe 

        }
    }
}
