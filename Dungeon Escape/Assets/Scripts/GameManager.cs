using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   //singleton
    private static GameManager instance;  
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Error");
            }
            
            return instance;
        }
    }

    public bool HasKeyCastle { get; set; }//property do sprawdzania czy gracz zakupi³ klucz
    private void Awake()
    {
        instance = this;
    }
}
