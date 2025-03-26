using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{
    public Text scoreText;//zmienna typu Text, która przechowuje referencjê do obiektu typu Text w scenie Unity. Komponent Text jest u¿ywany do wyœwietlania tekstu na ekranie.
    public Text highScore;//zmienna typu Text która przechowuje referencje do obiektu typu rexr w Scenie unity...
    public Text timerText;
    public Text timerText2;
    public Text comboText;
    public Text freezeTimeText;
    public float totalGameTime = 300f; // 5 minutes in seconds
    private bool isGameActive = true;
    public Text gameTimerText;
    public Image fadeImage;//zmienna typu Image, która przechowuje referencjê do obiektu typu Image w scenie Unity. Komponent Image jest u¿ywany do wyœwietlania obrazów (np. kolorowego panelu) na ekranie

    // Zmienna okreœlaj¹ca, czy aktywna jest zdolnoœæ podwajania wyniku.
    private bool is2xScoreActive = false;

    // Wspó³czynnik mno¿nika punktów.
    private float scoreMultiplier = 1f;

    // Czas trwania timera, który kontroluje aktywnoœæ zdolnoœci podwajania wyniku.
    private float timerDuration = 5f;

    // Aktualny stan timera.
    private float currentTimer;

    // Zmienna okreœlaj¹ca, czy aktywny jest wulkan (przyjêty w kodzie jako inny element gry).
    private bool isVolcanoActive = false;

    // Licznik kolejnych zdobytych punktów.
    private int consecutiveScoreCount = 0;

    // Czas ostatniego wykonanego ciêcia.
    private float lastSliceTime = 0f;

    // Czas miêdzy kolejnymi ciêciami, który pozwala na zaliczenie ich jako "kolejne" w ci¹gu.
    private float timeBetweenSlices = 0.5f;

    // Maksymalna liczba kolejnych zdobytych punktów, która aktywuje specjaln¹ zdolnoœæ.
    private int maxConsecutiveScoreCount = 6;

    private bool isFreezeActive = false;
    private float remainingFreezeTime = 0f;

    private Blade blade;//zmienna, która przechowuje referencjê do innego skryptu o nazwie "Blade". Ta zmienna jest u¿ywana do komunikacji i uzyskiwania danych z tego skryptu.
    private Spawner spawner;//mienna, która przechowuje referencjê do innego skryptu o nazwie "Spawner". Podobnie jak zmienna "blade", ta zmienna jest u¿ywana do komunikacji i uzyskiwania danych z tego skryptu.

    private int score;//zmienna do przechpowywania wyniku 
    public GameOver GameOverScreen;//zmienna do przechownia skryptu z GameOver
    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }//przypisanie referencji do komponentów 

    private void Start()
    {
        // Uruchomienie funkcji rozpoczêcia nowej gry.
        NewGame();

        // Odczytanie najlepszego wyniku z pamiêci urz¹dzenia i wyœwietlenie go na ekranie.
        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();

        // Ukrycie tekstu wyœwietlaj¹cego kombo (kolejne zdobyte punkty).
        comboText.gameObject.SetActive(false);

        // Przypisanie wartoœci pocz¹tkowej timera.
        currentTimer = timerDuration;
    }
    // Funkcja wywo³ywana automatycznie w ka¿dej klatce aktualizacji gry.
    private void Update()
    {
        if (isGameActive)
        {
            if (isFreezeActive)
            {
                // Zatrzymanie czasu tylko wtedy, gdy pozosta³y czas trwania zatrzymania jest wiêkszy od 0.
                if (remainingFreezeTime > 0f)
                {
                    remainingFreezeTime -= Time.deltaTime;
                    UpdateFreezeTimeDisplay();
                }
                else
                {
                    // Jeœli czas zatrzymania up³yn¹³, ustaw flagê isFreezeActive na false.
                    isFreezeActive = false;
                    UpdateFreezeTimeDisplay();
                }
            }
            else
            {
                totalGameTime -= Time.deltaTime;
            }
        }
        // Check if the game time has reached zero or below.
        if (totalGameTime <= 0f)
            {
                // End the game when the time is up.
                EndGame();
            }

        // Wywo³anie funkcji odpowiedzialnej za aktualizacjê wyœwietlania czasu timera.
        UpdateTimerDisplay();

        // Sprawdzenie, czy aktywna jest zdolnoœæ podwajania wyniku.
        if (is2xScoreActive)
        {
            // Zmniejszenie wartoœci timera o czas, jaki min¹³ od ostatniej klatki.
            currentTimer -= Time.deltaTime;

            // Sprawdzenie, czy timer osi¹gn¹³ wartoœæ mniejsz¹ od 0.
            if (currentTimer < 0f)
            {
                // Jeœli timer osi¹gn¹³ wartoœæ mniejsz¹ od 0, wy³¹czamy zdolnoœæ podwajania wyniku.
                is2xScoreActive = false;

                // Przywracamy wartoœæ mno¿nika punktów do wartoœci domyœlnej (1).
                scoreMultiplier = 1f;
            }
        }
        // Sprawdzenie, czy aktywny jest wulkan (lub inny element gry).
        else if (isVolcanoActive)
        {
            // Zmniejszenie wartoœci timera o czas, jaki min¹³ od ostatniej klatki.
            currentTimer -= Time.deltaTime;

            // Sprawdzenie, czy timer osi¹gn¹³ wartoœæ mniejsz¹ od 0.
            if (currentTimer < 0f)
            {
                // Jeœli timer osi¹gn¹³ wartoœæ mniejsz¹ od 0, wy³¹czamy aktywnoœæ wulkanu (lub innego elementu).
                isVolcanoActive = false;
            }
        }
    }
    private void EndGame()
    {
        isGameActive = false; // Stop the game.

        // Add any additional code for game over conditions here, such as saving high score.

        // Display the game over screen.
        Explode();
    }
    // Funkcja odpowiedzialna za aktualizacjê wyœwietlania czasu timera.
    private void UpdateTimerDisplay()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(totalGameTime / 60f);
            int seconds = Mathf.FloorToInt(totalGameTime % 60f);
            gameTimerText.text = "Time Left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        // Sprawdzenie, czy obiekt timerText nie jest null oraz czy zdolnoœæ podwajania wyniku jest aktywna (is2xScoreActive).
        if (timerText != null && is2xScoreActive)
        {
            // Jeœli powy¿szy warunek jest spe³niony, ustawiamy tekst wyœwietlany na ekranie, który informuje o pozosta³ym czasie timera z aktywn¹ zdolnoœci¹ podwajania wyniku.
            // Korzystamy z funkcji Mathf.RoundToInt() do zaokr¹glenia wartoœci currentTimer do liczby ca³kowitej i zamieniamy j¹ na string.
            timerText.text = "Time Left(2xscore): " + Mathf.RoundToInt(currentTimer).ToString();
        }
        else
        {
            // Jeœli obiekt timerText jest null lub zdolnoœæ podwajania wyniku nie jest aktywna, ustawiamy tekst wyœwietlany na pustego stringa,
            // co spowoduje ukrycie informacji o pozosta³ym czasie timera.
            timerText.text = "";
        }

        // Sprawdzenie, czy obiekt timerText2 nie jest null oraz czy zdolnoœæ wulkanu jest aktywna (isVolcanoActive).
        if (timerText2 != null && isVolcanoActive)
        {
            // Jeœli powy¿szy warunek jest spe³niony, ustawiamy tekst wyœwietlany na ekranie, który informuje o pozosta³ym czasie timera z aktywn¹ zdolnoœci¹ wulkanu.
            // Korzystamy z funkcji Mathf.RoundToInt() do zaokr¹glenia wartoœci currentTimer do liczby ca³kowitej i zamieniamy j¹ na string.
            timerText2.text = "Time Left(FruitVolcano): " + Mathf.RoundToInt(currentTimer).ToString();
        }
        else
        {
            // Jeœli obiekt timerText2 jest null lub zdolnoœæ wulkanu nie jest aktywna, ustawiamy tekst wyœwietlany na pustego stringa,
            // co spowoduje ukrycie informacji o pozosta³ym czasie timera zwi¹zanej z aktywnoœci¹ wulkanu.
            timerText2.text = "";
        }
        if (freezeTimeText != null && isFreezeActive)
        {
            freezeTimeText.text = "Freeze! Time Left: " + Mathf.CeilToInt(remainingFreezeTime).ToString();
        }
        else
        {
            freezeTimeText.text = "";
        }
    }
    private void NewGame()//funkcja nowa gra ustawiamy wszystko na 0 oraz uruchamiamy skrypty 
    {
        Time.timeScale = 1f;

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();//konwertowanie punktów na string 

        ClearScene();//wywo³¹nie czyszczenia planszy 

    }
    private void ClearScene()//funckaj do czyszczenia panelu gry 
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }//przechodzimy przez wszystkie elementy obiektów owoców czy bomb i po kolei je niszczymy 

    // Funkcja odpowiedzialna za zwiêkszanie wyniku i obs³ugê punktów zdobytych w ci¹gu.
    public void IncresaseScore()
    {
        // Pobranie aktualnego czasu podczas wykonania ciêcia (akcji gracza).
        float currentTime = Time.time;

        // Sprawdzenie, czy wyst¹pi³o wczeœniejsze ciêcie i czy nast¹pi³o w krótkim czasie od teraz.
        if (consecutiveScoreCount > 0 && currentTime - lastSliceTime < timeBetweenSlices)
        {
            // Jeœli tak, zwiêkszamy licznik kolejnych zdobytych punktów.
            consecutiveScoreCount++;
        }
        else
        {
            // Jeœli nie, resetujemy licznik kolejnych zdobytych punktów do 1.
            consecutiveScoreCount = 1;
        }

        // Zaktualizowanie czasu ostatniego wykonanego ciêcia.
        lastSliceTime = currentTime;

        // Zwiêkszenie aktualnego wyniku gry o 1 (podstawow¹ wartoœæ) pomno¿on¹ przez mno¿nik punktów.
        score += Mathf.RoundToInt(1 * scoreMultiplier);

        // Wyœwietlenie aktualnego wyniku na ekranie gry.
        scoreText.text = score.ToString();

        // Sprawdzenie, czy iloœæ kolejnych zdobytych punktów mieœci siê w po¿¹danym zakresie (3 do maxConsecutiveScoreCount).
        if (consecutiveScoreCount >= 3 && consecutiveScoreCount <= maxConsecutiveScoreCount)
        {
            // Jeœli tak, wyœwietlenie odpowiedniej wiadomoœci (komunikatu) na 1 sekundê.
            StartCoroutine(ShowConsecutiveScoreMessage(consecutiveScoreCount));
        }
        else if (consecutiveScoreCount > maxConsecutiveScoreCount)
        {
            // Obs³uga przypadku, gdy gracz przekroczy maksymaln¹ iloœæ kolejnych zdobytych punktów (opcjonalne).
            // Mo¿esz dodaæ dodatkowe efekty lub bonusy tutaj.
        }
    }

    // Funkcja korutyny (IEnumerator), która wyœwietla komunikat o zdobytych punktach w ci¹gu.
    private IEnumerator ShowConsecutiveScoreMessage(int count)
    {
        // Ustawienie odpowiedniej treœci komunikatu (combo) w zale¿noœci od iloœci kolejnych zdobytych punktów.
        comboText.text = count + "x Combo!";

        // Wyœwietlenie komunikatu na ekranie.
        comboText.gameObject.SetActive(true);

        // Poczekanie 1 sekundy (czas trwania komunikatu).
        yield return new WaitForSeconds(1f);

        // Ukrycie komunikatu po up³ywie czasu trwania.
        comboText.gameObject.SetActive(false);

        // Ponowne wyœwietlenie aktualnego wyniku na ekranie gry (po ukryciu komunikatu).
        scoreText.text = score.ToString();
    }
    // Funkcja odpowiedzialna za aktywacjê zdolnoœci podwajania wyniku (bananax2).
    public void Incresed2x()
    {
        // Sprawdzenie, czy zdolnoœæ podwajania wyniku nie jest ju¿ aktywna (is2xScoreActive == false).
        if (!is2xScoreActive)
        {
            // Jeœli zdolnoœæ nie jest aktywna, ustawiamy j¹ na aktywn¹ oraz zmieniamy mno¿nik punktów na 2.
            is2xScoreActive = true;
            scoreMultiplier = 2f;

            // Ustawienie czasu timera (currentTimer) na wartoœæ zdefiniowan¹ w zmiennej timerDuration.
            currentTimer = timerDuration;

            // Rozpoczêcie korutyny (IEnumerator) w celu wy³¹czenia zdolnoœci podwajania wyniku po zadanym opóŸnieniu.
            StartCoroutine(Disable2xScoreAfterDelay(5f));
        }
        else
        {
            // Jeœli zdolnoœæ jest ju¿ aktywna, resetujemy czas timera do wartoœci zdefiniowanej w zmiennej timerDuration.
            currentTimer = timerDuration;
        }
    }

    // Funkcja korutyny (IEnumerator), która wy³¹cza zdolnoœæ podwajania wyniku po zadanym opóŸnieniu.
    private IEnumerator Disable2xScoreAfterDelay(float delay)
    {
        // Poczekanie na opóŸnienie, które zosta³o przekazane do funkcji.
        yield return new WaitForSeconds(delay);

        // Po up³ywie opóŸnienia, wy³¹czamy zdolnoœæ podwajania wyniku, ustawiaj¹c odpowiednie zmienne na false i 1.
        is2xScoreActive = false;
        scoreMultiplier = 1f;
    }
    // Funkcja odpowiedzialna za aktywacjê zdolnoœci wulkanu.
    public void Volcano()
    {
        // Sprawdzenie, czy zdolnoœæ wulkanu nie jest ju¿ aktywna (isVolcanoActive == false).
        if (!isVolcanoActive)
        {
            // Jeœli zdolnoœæ nie jest aktywna, ustawiamy j¹ na aktywn¹.
            isVolcanoActive = true;

            // Wywo³anie metody SetVolcanoActive(true) na obiekcie spawner (prawdopodobnie odpowiedzialnym za tworzenie obiektów w grze),
            // aby uruchomiæ aktywnoœæ wulkanu na spawnerze.
            spawner.SetVolcanoActive(true);

            // Ustawienie czasu timera (currentTimer) na wartoœæ zdefiniowan¹ w zmiennej timerDuration.
            currentTimer = timerDuration;

            // Rozpoczêcie korutyny (IEnumerator) w celu wy³¹czenia aktywnoœci wulkanu po zadanym opóŸnieniu.
            StartCoroutine(DisableVolcanoAfterDelay(5f));
        }
        else
        {
            // Jeœli zdolnoœæ wulkanu jest ju¿ aktywna, resetujemy czas timera do wartoœci zdefiniowanej w zmiennej timerDuration,
            // co pozwoli przed³u¿yæ czas aktywnoœci zdolnoœci.
            currentTimer = timerDuration;
        }
    }

    // Funkcja korutyny (IEnumerator), która wy³¹cza aktywnoœæ wulkanu po zadanym opóŸnieniu.
    private IEnumerator DisableVolcanoAfterDelay(float delay)
    {
        // Poczekanie na opóŸnienie, które zosta³o przekazane do funkcji.
        yield return new WaitForSeconds(delay);

        // Po up³ywie opóŸnienia, wy³¹czamy aktywnoœæ wulkanu (zmieniaj¹c odpowiedni¹ zmienn¹ na false)
        // oraz wywo³ujemy metodê SetVolcanoActive(false) na obiekcie spawner, aby wy³¹czyæ aktywnoœæ wulkanu na spawnerze.
        isVolcanoActive = false;
        spawner.SetVolcanoActive(false);
    }
    public void Freeze()
    {
        if (!isFreezeActive)
        {
            isFreezeActive = true;
            // Ustawienie pozosta³ego czasu trwania zatrzymania na 5 sekund.
            remainingFreezeTime = 5f;
            UpdateFreezeTimeDisplay(); // Wyœwietlamy komunikat "Freeze!" na ekranie.
        }

    }
    private void UpdateFreezeTimeDisplay()
    {
        freezeTimeText.gameObject.SetActive(isFreezeActive);

    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;
        GameOverScreen.Setup(score);//aktywowanie funkcji setup z skryptu GameOver z przekazaniem do wysweitlania punktów 
        //wy³¹czanie funckji blade i spawner
        //StartCoroutine(ExplodeSequence());//uruchumienie korutyny 
    }
private IEnumerator ExplodeSequence()//Jest to korutyna, która odpowiada za sekwencjê dzia³añ podczas efektu eksplozji.
    {
        float elapsed = 0f;//zmienna przechowuj¹ca up³ywaj¹cy czas
        float duration = 0.5f;////zmienna przechowuj¹ca czas trwania eksplozji

        while (elapsed < duration) //petla porównój¹ca poprzednie zmienne 
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.black, t);
            //Oblicza wartoœæ t w zakresie od 0 do 1 na podstawie stosunku elapsed do duration. t jest u¿ywane jako parametr do funkcji Color.Lerp, która interpoluje miêdzy dwoma kolorami: Color.clear (przezroczysty) i Color.black (czarny). Ta interpolacja pozwala na p³ynne przejœcie od przezroczystoœci do koloru czarnego, co daje efekt zanikania ekranu.
            Time.timeScale = 1f - t;//Zmniejsza Time.timeScale z wartoœci 1 do 1f - t. Time.timeScale kontroluje prêdkoœæ czasu w grze. Zmniejszenie go powoduje zwolnienie akcji na scenie, co jest u¿yteczne do uzyskania efektu "spowolnienia" lub "zamro¿enia" gry w momencie eksplozji.
            elapsed += Time.unscaledDeltaTime;//Zwiêksza elapsed o czas trwania klatki (Time.unscaledDeltaTime) - wa¿ne jest, aby u¿ywaæ Time.unscaledDeltaTime, aby czas dzia³a³ niezale¿nie od zmiany wartoœci Time.timeScale

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);
        //Poza pêtl¹ while w korutynie jest opóŸnienie za pomoc¹ yield return new WaitForSecondsRealtime(1f). To opóŸnienie trwa 1 sekundê w czasie rzeczywistym, co pozwala na zatrzymanie ekranu na chwilê po eksplozji.
        NewGame();//wywo³anie nowej gry 

        elapsed = 0f;//reset up³ywu czasu

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.black, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }//Nastêpnie w drugiej pêtli while, która jest bardzo podobna do pierwszej, interpolujemy kolor ekranu z powrotem z koloru czarnego do przezroczystego, aby uzyskaæ efekt powrotu do normalnej rozgrywki.
    }
}
