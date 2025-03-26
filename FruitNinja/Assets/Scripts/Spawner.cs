using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea; //zmienna do referencji zeby kompoment collidrer m�g� sobie bra� jakis  komponent 
    public GameObject[] friutPrefabs; // tablica do losowania jaki owocek leci 

    public GameObject bombPrefab; 
    public float bombChance = 0.05f; //sznasa na spwan bomby

    public GameObject Score2xPrefab;
    public float Score2xChance = 0.05f; // sznasa na spawn banana X2 punkty 

    public GameObject MoreFruitsPrefab;
    public float MoreFruitsChance = 0.3f;

    public GameObject FreezeTimePrefab;
    public float FreezeTimeChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f; // min i max czas kiedy jeden owoc wylatuje a potem drugi

    public float minAngle = -15f;
    public float maxAngle = 15;//pod jakim k�tem b�d� lata� owocki 

    public float minForce = 18f;
    public float maxForce = 22f; // z jak� si�a lec� owocki 

    public float maxLifetime = 5f;// ile czasu b�d� istnia�y w grze bez akcji 

    private bool isVolcanoActive = false;
    private void Awake() //fucnkja unity kt�ra automatycznie wywo�uje skrypt kiedy sie �aduje 
    {
        spawnArea = GetComponent<Collider>(); //patrzy na ten sam lelemnt co skryp pracuje i pozwala nam wzi�c komponent na tym samym obieckie
    } //zrobiona referencja do colladera kt�ra jest spawn arena czyli sk�d wulatuj� owocki 

    private void OnEnable()//kolejna unuty funkcja kt�ra zostaje wywo�ana gry skrypt jest w��czony  
    {
        StartCoroutine(Spawn());//Coroutine jest funkcja wielozadaniowa kt�ra moze wykonywa� pewn� cz�c zadania potem sie wstrzyma� aby inne funckje czy cos dzia�a�y a potem doko�czy swoje dzia�anie 
    }

    private void OnDisable()//to samo co wyzej tylko jak wy��czony
    {
        StopAllCoroutines();//to zatrzymuje wszystkie wielozadaniowe funckje 
    }
//IEnumerator pozwala przechodzi� przez elementy tablicy/kolekcji ale nie wymaga przechowania wszystkiego na raz zamiast tego co� jest generowanie tylko  na dane ��danie
    private IEnumerator Spawn()
    {// kiedy ca�� funkcja jest typu IEnumerator natrafiaj�c na funckje yield return zwraca on dana wartosc czy cokolwiek i zapamietuje sw�j stan aby potem od tego miejsca kontunowac dzia�anie
        yield return new WaitForSeconds(2f);
    //yieldy s� tutaj jako korutyny czyli natrafiaj�c na ni� ona zwraca tutaj to czekanie 2sekundy zanim zacznie wykonywa� co jest nizej i zapamietuje ze tutaj to by�o leci co jest nizej i potem moze na nowo 
        while (enabled)// kiedy dzia�a b�dzie w niesko�czono�c to wykonywa�o 
        {
            GameObject prefab = friutPrefabs[Random.Range(0, friutPrefabs.Length)];//wybiera losowy objekt z przypietych do tablicy 

            if (Random.value < bombChance)//tworzenie obiektu bomby kiedy losowa warto�� jest mniejsza od zmiennej sznasy na bombe
            {
                prefab = bombPrefab;
            }

            if (Random.value < Score2xChance) //tworzenie obiektu banana kiedy losowa warto�� jest mniejsza od zmiennej sznasy na banana
            {
                prefab = Score2xPrefab;
            }
            if (Random.value < MoreFruitsChance)
            {
                prefab = MoreFruitsPrefab;
            }
            if (Random.value < FreezeTimeChance)
            {
                prefab = FreezeTimePrefab;
            }
            //Vector3 to funckja z unity pozwalajaca na ustalanie pozycji/punktu w tr�jwimiarze
            Vector3 position = new Vector3();//Tworzenie nowej zmiennej na pozycje tr�jwymiarow�
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            //Ustawiamy warto�� sk�adowej x w zmiennej position na losow� warto�� z przedzia�u mi�dzy minimalnym (min.x) i maksymalnym (max.x) zakresem wyznaczonym przez granice (bounds) obiektu spawnArea. Random.Range() to funkcja z Unity, kt�ra generuje losow� liczb� z danego zakresu 
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
            //Quaternion - funckja odpowidziala za matematyk� do obliczania danych wartosci itp, tworzymy z jej pomoca zmienna eulerowska do k�tu wystrza�u losujemy tylko wartosc dla z, bo tylko na nim pracujemy dla k�ta wystrza�y pozosta�e dwie zmienne si� nie zmieniaj� podczas tego 
            GameObject friut = Instantiate(prefab, position, rotation);//logika do faktycznego spawnowania owock�w
            //nowa zmienna fruit kt�ra przechowuje referencje do nowo utworzonych obiekt�w samo GameObject to podstawowa zmienna w unity na obiekty w grze 
            //Instantiate pozwala na tworzenie kopii i przyjmuje parametry prefab czyli oryina� sk�d pobiera kopie pozycje czyli gdzie go utworzyc i rotacje czyli jak bedzie obr�cony 
            Destroy(friut, maxLifetime);//niszczy owocki kt�re d�uzej s� niz zmienna maxLifetime

            float force = Random.Range(minForce, maxForce);//Tworzenie nowej zmiennej przechowuj�cej losowa warto�� si�y wystrzeliwania owock�w pomi�dzy zmiennymi
            friut.GetComponent<Rigidbody>().AddForce(friut.transform.up * force, ForceMode.Impulse);
            //friut.GetComponent<Rigidbody>() uzyskiwanie dost�pu owockom do rigidbody kt�ry odpowaida na symulacje ruchu i og�lnie fizyki w grze 
            //.AddForce(friut.transform.up * force dodajemy si�e wyrzutu do rigidbody dajemy j� do g�ry bo tam lec� owocki potem tylko domnozenie przez wczesniej losowana wartosc Force kt�ra ustala jak leci owocek
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));// czeka dan� liczbe sekund aby m�g� wykona� p�tle na nowo w k�ko w tyym przypadku sobie to losuje z przedzia�u ze wcze�niejszych zmiennych
        }
    }
    public void SetVolcanoActive(bool isActive)
    {
        if (isActive)
        {
            maxSpawnDelay = 0.50f;
            bombChance = 0.05f;
            Score2xChance = 0.01f;
            MoreFruitsChance = 0.01f;
        }
        else
        {
            // Je�li Volcano nie jest aktywny, przywracamy oryginalne warto�ci
            maxSpawnDelay = 1f;
            bombChance = 0.25f;
            Score2xChance = 0.05f;
            MoreFruitsChance = 0.03f;
        }

        isVolcanoActive = isActive;

        if (isActive)
        {
            StartCoroutine(ResetVolcanoAfterDelay(5f));
        }
    }

    private IEnumerator ResetVolcanoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Po up�ywie czasu delay przywracamy oryginalne warto�ci
        maxSpawnDelay = 1f;
        bombChance = 0.25f;
        Score2xChance = 0.05f;
        MoreFruitsChance = 0.03f;

        isVolcanoActive = false;
    }
}
