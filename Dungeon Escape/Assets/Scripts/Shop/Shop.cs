using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ShopPanel;//zmienna do przypisania panelu sklepu w silniku
    public int CurrentSelectedItem;//zmienna jaki item jb�dzie wybrany
    public int CurrentItemCost;//zmienna przekowuj�ca koszt danego itemka

    private Player player;//uchwyt do przypisania do niego skryptu gracza
    private void OnTriggerEnter2D(Collider2D other)//funkcja do wykrywania kolizji
    {
        if(other.tag == "Player")//je�eli zderzy si� z graczem
        {
            player = other.GetComponent<Player>();//uchwyt odrazu przypisany do skryptu gracza 
            
            if(player != null)//je�li ten gracz istnieje
            {
                UIManager.Instance.OpenShop(player.diamonds);//z UIManagera wywo�ujemy funkcje OpenShope i przekazujemy warto�c diament� gracza
            }
            ShopPanel.SetActive(true);//ustawia panel na w��czony
        }
    }

    private void OnTriggerExit2D(Collider2D other)//funkcja do wykrywania wyj�cie ,,other" z pola triggerowanego
    {
        if (other.tag == "Player")//je�eli other to gracz
        {
            ShopPanel.SetActive(false);//to po wyj�ciu ze strefy wy��czamy sklep
        }
    }

    public void SelectItem(int item)//funckja do trzymania logiki do kleikniecia przycisku (w silniku nale�y doda� onclickEvent, wybra� obiekt, kt�e ma skrypt sklepu i tam ywbra�, �e na kliekneociu ma wywo��� t� metode) 
    {//b�dziemy przekazywac do tej funckji inta, kt�ry b�dzie reprezentowa� dany itemek, liczac od 0 miesz itd
        switch (item)//switch na podstawie itemku
        {
            case 0:
                UIManager.Instance.UpdateShopSelection(122);//wywo�ujemy z UIManagewra metode UpdateShop... i przekazujemy warto�c Y jak� chcemy
                //i tak po kolei dla ka�dego itemka
                CurrentSelectedItem = 0;//przypisanie do zmiennej ID dla danego itemka
                CurrentItemCost = 200;//przypisanie ceny dla ka�dego itemka
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
    public void BuyItem()//metoda do przechowywania logiki kupowania itemk�w
    {
        if (player.diamonds >= CurrentItemCost)//je�eli gracz ma tyle ile trzbea lub wi�cej gem�w
        {
            if (CurrentSelectedItem == 2)//je�eli wybranym itemkiem jest klucz
            {
                GameManager.Instance.HasKeyCastle = true;//wywo�ujemy metode w wGameManagerze. kt�ra ustawia posiadanie klucza na true
            }
            player.diamonds -= CurrentItemCost;//odj�cie gems�w o cene itemka

            ShopPanel.SetActive(false);//zamyka si� sklep
        }
        else
        {//jak go nie sta� 
            Debug.Log("PoorGuy");
            ShopPanel.SetActive(false);//zamyka si� sklep
        }
    }
}
