using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//biblioteka do rzeczy zwi�zanych z UI takich jak text 

public class UIManager : MonoBehaviour
{
    //singleton, dok�adny opis tego, jest w grze fleka 

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Error no UI");
            }
            return _instance;
        }
    }

    public Text PlayerGemCount;//zmienna tekstowa do aktualizacji stanu gem�w gracza, przypisa� w silniku 
    public Image SelectionImg;//zmienna obrazkowa do trzymania pood�wietlania wybranego itemka. przypisa� w silniku
    public Text GemCountText;//zmienna tekstowa do zarz�dania wy�wieltaniem liczby gem�w gracza, przypisa� w silniku
    public Image[] healthBars;//zamienna tablicowa do trzymania wszystkich komponent�w od �ycia. przypisa� w unity
    private void Awake()
    {
        _instance = this;
    }
    public void OpenShop(int gemCoount)//funcja, kt�ra b�dzie s�u�y�a do komunikacji z skryptem gracza i przypisywa�a dan� liczbe gem�w
    {//u�yjemy tej funcki do komunikacji z skryptem gracza od gem�w a ta przeka�e t� warto�� tutaj 
        PlayerGemCount.text = "Gems: " + gemCoount;//Wypisanie w tym polu tekstu + przekazanej warto�ci
    }

    public void UpdateShopSelection(int yPos)//metoda do ustawiania miejsca naszego pod�wietlania 
    {
        SelectionImg.rectTransform.anchoredPosition = new Vector2(SelectionImg.rectTransform.anchoredPosition.x, yPos);
        //gotowa funkcja unity do ustawiania nowych warto�ci, kt�ra przyjmuje nowe x i y, w tym moemencie nie obchodzi nas modyfikacja
        //x wi�c jest on usationy na bazowy za pomoc� tej samej funkcji
    }

    public void UpdateGameCount(int count)//funkcja do zarz�dania poprawnym wy�wietlaniem liczby gem�w gracza
    {
        GemCountText.text = "" + count;//do tego tekstu przekazujemy warto�� gem�w 
    }

    public void UpdateLives(int lives)//funkcja do zarz�dania wy�wietlaniem poprawy m stanu zdrowia gracza
    {
        for(int i=0; i<=lives; i++)//p�tla przechodz�ca przez ka�dy element paska �ycia(s� 4 �ycia i leci po kolei)
        {
            if(i == lives)//je�eli i jest r�wne �yciom
            {
                healthBars[i].enabled = false;//wy��czamy z UI ten dany element paska �ycia

            }
        }
    }
}
