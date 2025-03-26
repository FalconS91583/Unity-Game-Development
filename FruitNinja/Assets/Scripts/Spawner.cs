using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea; //zmienna do referencji zeby kompoment collidrer móg³ sobie braæ jakis  komponent 
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
    public float maxAngle = 15;//pod jakim k¹tem bêd¹ lataæ owocki 

    public float minForce = 18f;
    public float maxForce = 22f; // z jak¹ si³a lec¹ owocki 

    public float maxLifetime = 5f;// ile czasu bêd¹ istnia³y w grze bez akcji 

    private bool isVolcanoActive = false;
    private void Awake() //fucnkja unity która automatycznie wywo³uje skrypt kiedy sie ³aduje 
    {
        spawnArea = GetComponent<Collider>(); //patrzy na ten sam lelemnt co skryp pracuje i pozwala nam wzi¹c komponent na tym samym obieckie
    } //zrobiona referencja do colladera która jest spawn arena czyli sk¹d wulatuj¹ owocki 

    private void OnEnable()//kolejna unuty funkcja która zostaje wywo³ana gry skrypt jest w³¹czony  
    {
        StartCoroutine(Spawn());//Coroutine jest funkcja wielozadaniowa która moze wykonywaæ pewn¹ czêœc zadania potem sie wstrzymaæ aby inne funckje czy cos dzia³a³y a potem dokoñczy swoje dzia³anie 
    }

    private void OnDisable()//to samo co wyzej tylko jak wy³¹czony
    {
        StopAllCoroutines();//to zatrzymuje wszystkie wielozadaniowe funckje 
    }
//IEnumerator pozwala przechodziæ przez elementy tablicy/kolekcji ale nie wymaga przechowania wszystkiego na raz zamiast tego coœ jest generowanie tylko  na dane ¿¹danie
    private IEnumerator Spawn()
    {// kiedy ca³¹ funkcja jest typu IEnumerator natrafiaj¹c na funckje yield return zwraca on dana wartosc czy cokolwiek i zapamietuje swój stan aby potem od tego miejsca kontunowac dzia³anie
        yield return new WaitForSeconds(2f);
    //yieldy s¹ tutaj jako korutyny czyli natrafiaj¹c na ni¹ ona zwraca tutaj to czekanie 2sekundy zanim zacznie wykonywaæ co jest nizej i zapamietuje ze tutaj to by³o leci co jest nizej i potem moze na nowo 
        while (enabled)// kiedy dzia³a bêdzie w nieskoñczonoœc to wykonywa³o 
        {
            GameObject prefab = friutPrefabs[Random.Range(0, friutPrefabs.Length)];//wybiera losowy objekt z przypietych do tablicy 

            if (Random.value < bombChance)//tworzenie obiektu bomby kiedy losowa wartoœæ jest mniejsza od zmiennej sznasy na bombe
            {
                prefab = bombPrefab;
            }

            if (Random.value < Score2xChance) //tworzenie obiektu banana kiedy losowa wartoœæ jest mniejsza od zmiennej sznasy na banana
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
            //Vector3 to funckja z unity pozwalajaca na ustalanie pozycji/punktu w trójwimiarze
            Vector3 position = new Vector3();//Tworzenie nowej zmiennej na pozycje trójwymiarow¹
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            //Ustawiamy wartoœæ sk³adowej x w zmiennej position na losow¹ wartoœæ z przedzia³u miêdzy minimalnym (min.x) i maksymalnym (max.x) zakresem wyznaczonym przez granice (bounds) obiektu spawnArea. Random.Range() to funkcja z Unity, która generuje losow¹ liczbê z danego zakresu 
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
            //Quaternion - funckja odpowidziala za matematykê do obliczania danych wartosci itp, tworzymy z jej pomoca zmienna eulerowska do k¹tu wystrza³u losujemy tylko wartosc dla z, bo tylko na nim pracujemy dla k¹ta wystrza³y pozosta³e dwie zmienne siê nie zmieniaj¹ podczas tego 
            GameObject friut = Instantiate(prefab, position, rotation);//logika do faktycznego spawnowania owocków
            //nowa zmienna fruit która przechowuje referencje do nowo utworzonych obiektów samo GameObject to podstawowa zmienna w unity na obiekty w grze 
            //Instantiate pozwala na tworzenie kopii i przyjmuje parametry prefab czyli oryina³ sk¹d pobiera kopie pozycje czyli gdzie go utworzyc i rotacje czyli jak bedzie obrócony 
            Destroy(friut, maxLifetime);//niszczy owocki które d³uzej s¹ niz zmienna maxLifetime

            float force = Random.Range(minForce, maxForce);//Tworzenie nowej zmiennej przechowuj¹cej losowa wartoœæ si³y wystrzeliwania owocków pomiêdzy zmiennymi
            friut.GetComponent<Rigidbody>().AddForce(friut.transform.up * force, ForceMode.Impulse);
            //friut.GetComponent<Rigidbody>() uzyskiwanie dostêpu owockom do rigidbody który odpowaida na symulacje ruchu i ogólnie fizyki w grze 
            //.AddForce(friut.transform.up * force dodajemy si³e wyrzutu do rigidbody dajemy j¹ do góry bo tam lec¹ owocki potem tylko domnozenie przez wczesniej losowana wartosc Force która ustala jak leci owocek
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));// czeka dan¹ liczbe sekund aby móg³ wykonaæ pêtle na nowo w kó³ko w tyym przypadku sobie to losuje z przedzia³u ze wczeœniejszych zmiennych
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
            // Jeœli Volcano nie jest aktywny, przywracamy oryginalne wartoœci
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

        // Po up³ywie czasu delay przywracamy oryginalne wartoœci
        maxSpawnDelay = 1f;
        bombChance = 0.25f;
        Score2xChance = 0.05f;
        MoreFruitsChance = 0.03f;

        isVolcanoActive = false;
    }
}
