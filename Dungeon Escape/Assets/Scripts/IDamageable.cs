using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable //jest to nowy obieky interfejs
    //onterfejs polega na tym, ¿e jest swojego rodzaju umow¹, gdzie ka¿dy obiekt w grze bêdzie mia³ na sobie ten interfejs
    //bêdzie go wykorzystywa³ w tym mprzypadku, ¿e zawsze jak gracz walnie mieczem i jakiœ obiekt czy przeciwnik bêdzie mia³ ten skrypt 
    //powie, ¿e hej zosta³em zaatakowany przez gracza i  ten obiekt niewa¿ne co musi u¿ywaæ tej funkcji
{
    int health { get; set; }//w interfejsach nie mo¿emy u¿ywaæ zwyk³ych zmiennych. potrzbne s¹ parametry, które s¹ jak zaawansowane 
    //zmienne, pobieramy jak¹œ wartoœæ i ustawiamy j¹ na nowo
    void Damage();//funcka do obra¿eñ
    

    
}
