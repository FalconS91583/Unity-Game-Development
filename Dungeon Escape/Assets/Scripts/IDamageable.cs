using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable //jest to nowy obieky interfejs
    //onterfejs polega na tym, �e jest swojego rodzaju umow�, gdzie ka�dy obiekt w grze b�dzie mia� na sobie ten interfejs
    //b�dzie go wykorzystywa� w tym mprzypadku, �e zawsze jak gracz walnie mieczem i jaki� obiekt czy przeciwnik b�dzie mia� ten skrypt 
    //powie, �e hej zosta�em zaatakowany przez gracza i  ten obiekt niewa�ne co musi u�ywa� tej funkcji
{
    int health { get; set; }//w interfejsach nie mo�emy u�ywa� zwyk�ych zmiennych. potrzbne s� parametry, kt�re s� jak zaawansowane 
    //zmienne, pobieramy jak�� warto�� i ustawiamy j� na nowo
    void Damage();//funcka do obra�e�
    

    
}
