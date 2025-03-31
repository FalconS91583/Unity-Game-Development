using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//biblioteka do rzeczy zwi¹zanych z UI takich jak text 

public class UIManager : MonoBehaviour
{
    //singleton, dok³adny opis tego, jest w grze fleka 

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

    public Text PlayerGemCount;//zmienna tekstowa do aktualizacji stanu gemów gracza, przypisaæ w silniku 
    public Image SelectionImg;//zmienna obrazkowa do trzymania poodœwietlania wybranego itemka. przypisaæ w silniku
    public Text GemCountText;//zmienna tekstowa do zarz¹dania wyœwieltaniem liczby gemów gracza, przypisaæ w silniku
    public Image[] healthBars;//zamienna tablicowa do trzymania wszystkich komponentów od ¿ycia. przypisaæ w unity
    private void Awake()
    {
        _instance = this;
    }
    public void OpenShop(int gemCoount)//funcja, która bêdzie s³u¿y³a do komunikacji z skryptem gracza i przypisywa³a dan¹ liczbe gemów
    {//u¿yjemy tej funcki do komunikacji z skryptem gracza od gemów a ta przeka¿e t¹ wartoœæ tutaj 
        PlayerGemCount.text = "Gems: " + gemCoount;//Wypisanie w tym polu tekstu + przekazanej wartoœci
    }

    public void UpdateShopSelection(int yPos)//metoda do ustawiania miejsca naszego podœwietlania 
    {
        SelectionImg.rectTransform.anchoredPosition = new Vector2(SelectionImg.rectTransform.anchoredPosition.x, yPos);
        //gotowa funkcja unity do ustawiania nowych wartoœci, która przyjmuje nowe x i y, w tym moemencie nie obchodzi nas modyfikacja
        //x wiêc jest on usationy na bazowy za pomoc¹ tej samej funkcji
    }

    public void UpdateGameCount(int count)//funkcja do zarz¹dania poprawnym wyœwietlaniem liczby gemów gracza
    {
        GemCountText.text = "" + count;//do tego tekstu przekazujemy wartoœæ gemów 
    }

    public void UpdateLives(int lives)//funkcja do zarz¹dania wyœwietlaniem poprawy m stanu zdrowia gracza
    {
        for(int i=0; i<=lives; i++)//pêtla przechodz¹ca przez ka¿dy element paska ¿ycia(s¹ 4 ¿ycia i leci po kolei)
        {
            if(i == lives)//je¿eli i jest równe ¿yciom
            {
                healthBars[i].enabled = false;//wy³¹czamy z UI ten dany element paska ¿ycia

            }
        }
    }
}
