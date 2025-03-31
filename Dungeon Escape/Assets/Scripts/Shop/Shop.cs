using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ShopPanel;//zmienna do przypisania panelu sklepu w silniku
    public int CurrentSelectedItem;//zmienna jaki item jbêdzie wybrany
    public int CurrentItemCost;//zmienna przekowuj¹ca koszt danego itemka

    private Player player;//uchwyt do przypisania do niego skryptu gracza
    private void OnTriggerEnter2D(Collider2D other)//funkcja do wykrywania kolizji
    {
        if(other.tag == "Player")//je¿eli zderzy siê z graczem
        {
            player = other.GetComponent<Player>();//uchwyt odrazu przypisany do skryptu gracza 
            
            if(player != null)//je¿li ten gracz istnieje
            {
                UIManager.Instance.OpenShop(player.diamonds);//z UIManagera wywo³ujemy funkcje OpenShope i przekazujemy wartoœc diamentó gracza
            }
            ShopPanel.SetActive(true);//ustawia panel na w³¹czony
        }
    }

    private void OnTriggerExit2D(Collider2D other)//funkcja do wykrywania wyjœcie ,,other" z pola triggerowanego
    {
        if (other.tag == "Player")//je¿eli other to gracz
        {
            ShopPanel.SetActive(false);//to po wyjœciu ze strefy wy³¹czamy sklep
        }
    }

    public void SelectItem(int item)//funckja do trzymania logiki do kleikniecia przycisku (w silniku nale¿y dodaæ onclickEvent, wybraæ obiekt, któe ma skrypt sklepu i tam ywbraæ, ¿e na kliekneociu ma wywo³¹æ t¹ metode) 
    {//bêdziemy przekazywac do tej funckji inta, który bêdzie reprezentowa³ dany itemek, liczac od 0 miesz itd
        switch (item)//switch na podstawie itemku
        {
            case 0:
                UIManager.Instance.UpdateShopSelection(122);//wywo³ujemy z UIManagewra metode UpdateShop... i przekazujemy wartoœc Y jak¹ chcemy
                //i tak po kolei dla ka¿dego itemka
                CurrentSelectedItem = 0;//przypisanie do zmiennej ID dla danego itemka
                CurrentItemCost = 200;//przypisanie ceny dla ka¿dego itemka
                break;
            case 1:
                UIManager.Instance.UpdateShopSelection(-6);
                CurrentSelectedItem = 1;
                CurrentItemCost = 400;
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(-122);
                CurrentSelectedItem = 2;
                CurrentItemCost = 100;
                break;

        }
    }
    public void BuyItem()//metoda do przechowywania logiki kupowania itemków
    {
        if (player.diamonds >= CurrentItemCost)//je¿eli gracz ma tyle ile trzbea lub wiêcej gemów
        {
            if (CurrentSelectedItem == 2)//je¿eli wybranym itemkiem jest klucz
            {
                GameManager.Instance.HasKeyCastle = true;//wywo³ujemy metode w wGameManagerze. która ustawia posiadanie klucza na true
            }
            player.diamonds -= CurrentItemCost;//odjêcie gemsów o cene itemka

            ShopPanel.SetActive(false);//zamyka siê sklep
        }
        else
        {//jak go nie staæ 
            Debug.Log("PoorGuy");
            ShopPanel.SetActive(false);//zamyka siê sklep
        }
    }
}
