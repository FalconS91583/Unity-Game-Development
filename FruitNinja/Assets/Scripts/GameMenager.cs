using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{
    public Text scoreText;//zmienna typu Text, kt�ra przechowuje referencj� do obiektu typu Text w scenie Unity. Komponent Text jest u�ywany do wy�wietlania tekstu na ekranie.
    public Text highScore;//zmienna typu Text kt�ra przechowuje referencje do obiektu typu rexr w Scenie unity...
    public Text timerText;
    public Text timerText2;
    public Text comboText;
    public Text freezeTimeText;
    public float totalGameTime = 300f; // 5 minutes in seconds
    private bool isGameActive = true;
    public Text gameTimerText;
    public Image fadeImage;//zmienna typu Image, kt�ra przechowuje referencj� do obiektu typu Image w scenie Unity. Komponent Image jest u�ywany do wy�wietlania obraz�w (np. kolorowego panelu) na ekranie

    // Zmienna okre�laj�ca, czy aktywna jest zdolno�� podwajania wyniku.
    private bool is2xScoreActive = false;

    // Wsp�czynnik mno�nika punkt�w.
    private float scoreMultiplier = 1f;

    // Czas trwania timera, kt�ry kontroluje aktywno�� zdolno�ci podwajania wyniku.
    private float timerDuration = 5f;

    // Aktualny stan timera.
    private float currentTimer;

    // Zmienna okre�laj�ca, czy aktywny jest wulkan (przyj�ty w kodzie jako inny element gry).
    private bool isVolcanoActive = false;

    // Licznik kolejnych zdobytych punkt�w.
    private int consecutiveScoreCount = 0;

    // Czas ostatniego wykonanego ci�cia.
    private float lastSliceTime = 0f;

    // Czas mi�dzy kolejnymi ci�ciami, kt�ry pozwala na zaliczenie ich jako "kolejne" w ci�gu.
    private float timeBetweenSlices = 0.5f;

    // Maksymalna liczba kolejnych zdobytych punkt�w, kt�ra aktywuje specjaln� zdolno��.
    private int maxConsecutiveScoreCount = 6;

    private bool isFreezeActive = false;
    private float remainingFreezeTime = 0f;

    private Blade blade;//zmienna, kt�ra przechowuje referencj� do innego skryptu o nazwie "Blade". Ta zmienna jest u�ywana do komunikacji i uzyskiwania danych z tego skryptu.
    private Spawner spawner;//mienna, kt�ra przechowuje referencj� do innego skryptu o nazwie "Spawner". Podobnie jak zmienna "blade", ta zmienna jest u�ywana do komunikacji i uzyskiwania danych z tego skryptu.

    private int score;//zmienna do przechpowywania wyniku 
    public GameOver GameOverScreen;//zmienna do przechownia skryptu z GameOver
    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }//przypisanie referencji do komponent�w 

    private void Start()
    {
        // Uruchomienie funkcji rozpocz�cia nowej gry.
        NewGame();

        // Odczytanie najlepszego wyniku z pami�ci urz�dzenia i wy�wietlenie go na ekranie.
        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();

        // Ukrycie tekstu wy�wietlaj�cego kombo (kolejne zdobyte punkty).
        comboText.gameObject.SetActive(false);

        // Przypisanie warto�ci pocz�tkowej timera.
        currentTimer = timerDuration;
    }
    // Funkcja wywo�ywana automatycznie w ka�dej klatce aktualizacji gry.
    private void Update()
    {
        if (isGameActive)
        {
            if (isFreezeActive)
            {
                // Zatrzymanie czasu tylko wtedy, gdy pozosta�y czas trwania zatrzymania jest wi�kszy od 0.
                if (remainingFreezeTime > 0f)
                {
                    remainingFreezeTime -= Time.deltaTime;
                    UpdateFreezeTimeDisplay();
                }
                else
                {
                    // Je�li czas zatrzymania up�yn��, ustaw flag� isFreezeActive na false.
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

        // Wywo�anie funkcji odpowiedzialnej za aktualizacj� wy�wietlania czasu timera.
        UpdateTimerDisplay();

        // Sprawdzenie, czy aktywna jest zdolno�� podwajania wyniku.
        if (is2xScoreActive)
        {
            // Zmniejszenie warto�ci timera o czas, jaki min�� od ostatniej klatki.
            currentTimer -= Time.deltaTime;

            // Sprawdzenie, czy timer osi�gn�� warto�� mniejsz� od 0.
            if (currentTimer < 0f)
            {
                // Je�li timer osi�gn�� warto�� mniejsz� od 0, wy��czamy zdolno�� podwajania wyniku.
                is2xScoreActive = false;

                // Przywracamy warto�� mno�nika punkt�w do warto�ci domy�lnej (1).
                scoreMultiplier = 1f;
            }
        }
        // Sprawdzenie, czy aktywny jest wulkan (lub inny element gry).
        else if (isVolcanoActive)
        {
            // Zmniejszenie warto�ci timera o czas, jaki min�� od ostatniej klatki.
            currentTimer -= Time.deltaTime;

            // Sprawdzenie, czy timer osi�gn�� warto�� mniejsz� od 0.
            if (currentTimer < 0f)
            {
                // Je�li timer osi�gn�� warto�� mniejsz� od 0, wy��czamy aktywno�� wulkanu (lub innego elementu).
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
    // Funkcja odpowiedzialna za aktualizacj� wy�wietlania czasu timera.
    private void UpdateTimerDisplay()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(totalGameTime / 60f);
            int seconds = Mathf.FloorToInt(totalGameTime % 60f);
            gameTimerText.text = "Time Left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        // Sprawdzenie, czy obiekt timerText nie jest null oraz czy zdolno�� podwajania wyniku jest aktywna (is2xScoreActive).
        if (timerText != null && is2xScoreActive)
        {
            // Je�li powy�szy warunek jest spe�niony, ustawiamy tekst wy�wietlany na ekranie, kt�ry informuje o pozosta�ym czasie timera z aktywn� zdolno�ci� podwajania wyniku.
            // Korzystamy z funkcji Mathf.RoundToInt() do zaokr�glenia warto�ci currentTimer do liczby ca�kowitej i zamieniamy j� na string.
            timerText.text = "Time Left(2xscore): " + Mathf.RoundToInt(currentTimer).ToString();
        }
        else
        {
            // Je�li obiekt timerText jest null lub zdolno�� podwajania wyniku nie jest aktywna, ustawiamy tekst wy�wietlany na pustego stringa,
            // co spowoduje ukrycie informacji o pozosta�ym czasie timera.
            timerText.text = "";
        }

        // Sprawdzenie, czy obiekt timerText2 nie jest null oraz czy zdolno�� wulkanu jest aktywna (isVolcanoActive).
        if (timerText2 != null && isVolcanoActive)
        {
            // Je�li powy�szy warunek jest spe�niony, ustawiamy tekst wy�wietlany na ekranie, kt�ry informuje o pozosta�ym czasie timera z aktywn� zdolno�ci� wulkanu.
            // Korzystamy z funkcji Mathf.RoundToInt() do zaokr�glenia warto�ci currentTimer do liczby ca�kowitej i zamieniamy j� na string.
            timerText2.text = "Time Left(FruitVolcano): " + Mathf.RoundToInt(currentTimer).ToString();
        }
        else
        {
            // Je�li obiekt timerText2 jest null lub zdolno�� wulkanu nie jest aktywna, ustawiamy tekst wy�wietlany na pustego stringa,
            // co spowoduje ukrycie informacji o pozosta�ym czasie timera zwi�zanej z aktywno�ci� wulkanu.
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
        scoreText.text = score.ToString();//konwertowanie punkt�w na string 

        ClearScene();//wywo��nie czyszczenia planszy 

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
    }//przechodzimy przez wszystkie elementy obiekt�w owoc�w czy bomb i po kolei je niszczymy 

    // Funkcja odpowiedzialna za zwi�kszanie wyniku i obs�ug� punkt�w zdobytych w ci�gu.
    public void IncresaseScore()
    {
        // Pobranie aktualnego czasu podczas wykonania ci�cia (akcji gracza).
        float currentTime = Time.time;

        // Sprawdzenie, czy wyst�pi�o wcze�niejsze ci�cie i czy nast�pi�o w kr�tkim czasie od teraz.
        if (consecutiveScoreCount > 0 && currentTime - lastSliceTime < timeBetweenSlices)
        {
            // Je�li tak, zwi�kszamy licznik kolejnych zdobytych punkt�w.
            consecutiveScoreCount++;
        }
        else
        {
            // Je�li nie, resetujemy licznik kolejnych zdobytych punkt�w do 1.
            consecutiveScoreCount = 1;
        }

        // Zaktualizowanie czasu ostatniego wykonanego ci�cia.
        lastSliceTime = currentTime;

        // Zwi�kszenie aktualnego wyniku gry o 1 (podstawow� warto��) pomno�on� przez mno�nik punkt�w.
        score += Mathf.RoundToInt(1 * scoreMultiplier);

        // Wy�wietlenie aktualnego wyniku na ekranie gry.
        scoreText.text = score.ToString();

        // Sprawdzenie, czy ilo�� kolejnych zdobytych punkt�w mie�ci si� w po��danym zakresie (3 do maxConsecutiveScoreCount).
        if (consecutiveScoreCount >= 3 && consecutiveScoreCount <= maxConsecutiveScoreCount)
        {
            // Je�li tak, wy�wietlenie odpowiedniej wiadomo�ci (komunikatu) na 1 sekund�.
            StartCoroutine(ShowConsecutiveScoreMessage(consecutiveScoreCount));
        }
        else if (consecutiveScoreCount > maxConsecutiveScoreCount)
        {
            // Obs�uga przypadku, gdy gracz przekroczy maksymaln� ilo�� kolejnych zdobytych punkt�w (opcjonalne).
            // Mo�esz doda� dodatkowe efekty lub bonusy tutaj.
        }
    }

    // Funkcja korutyny (IEnumerator), kt�ra wy�wietla komunikat o zdobytych punktach w ci�gu.
    private IEnumerator ShowConsecutiveScoreMessage(int count)
    {
        // Ustawienie odpowiedniej tre�ci komunikatu (combo) w zale�no�ci od ilo�ci kolejnych zdobytych punkt�w.
        comboText.text = count + "x Combo!";

        // Wy�wietlenie komunikatu na ekranie.
        comboText.gameObject.SetActive(true);

        // Poczekanie 1 sekundy (czas trwania komunikatu).
        yield return new WaitForSeconds(1f);

        // Ukrycie komunikatu po up�ywie czasu trwania.
        comboText.gameObject.SetActive(false);

        // Ponowne wy�wietlenie aktualnego wyniku na ekranie gry (po ukryciu komunikatu).
        scoreText.text = score.ToString();
    }
    // Funkcja odpowiedzialna za aktywacj� zdolno�ci podwajania wyniku (bananax2).
    public void Incresed2x()
    {
        // Sprawdzenie, czy zdolno�� podwajania wyniku nie jest ju� aktywna (is2xScoreActive == false).
        if (!is2xScoreActive)
        {
            // Je�li zdolno�� nie jest aktywna, ustawiamy j� na aktywn� oraz zmieniamy mno�nik punkt�w na 2.
            is2xScoreActive = true;
            scoreMultiplier = 2f;

            // Ustawienie czasu timera (currentTimer) na warto�� zdefiniowan� w zmiennej timerDuration.
            currentTimer = timerDuration;

            // Rozpocz�cie korutyny (IEnumerator) w celu wy��czenia zdolno�ci podwajania wyniku po zadanym op�nieniu.
            StartCoroutine(Disable2xScoreAfterDelay(5f));
        }
        else
        {
            // Je�li zdolno�� jest ju� aktywna, resetujemy czas timera do warto�ci zdefiniowanej w zmiennej timerDuration.
            currentTimer = timerDuration;
        }
    }

    // Funkcja korutyny (IEnumerator), kt�ra wy��cza zdolno�� podwajania wyniku po zadanym op�nieniu.
    private IEnumerator Disable2xScoreAfterDelay(float delay)
    {
        // Poczekanie na op�nienie, kt�re zosta�o przekazane do funkcji.
        yield return new WaitForSeconds(delay);

        // Po up�ywie op�nienia, wy��czamy zdolno�� podwajania wyniku, ustawiaj�c odpowiednie zmienne na false i 1.
        is2xScoreActive = false;
        scoreMultiplier = 1f;
    }
    // Funkcja odpowiedzialna za aktywacj� zdolno�ci wulkanu.
    public void Volcano()
    {
        // Sprawdzenie, czy zdolno�� wulkanu nie jest ju� aktywna (isVolcanoActive == false).
        if (!isVolcanoActive)
        {
            // Je�li zdolno�� nie jest aktywna, ustawiamy j� na aktywn�.
            isVolcanoActive = true;

            // Wywo�anie metody SetVolcanoActive(true) na obiekcie spawner (prawdopodobnie odpowiedzialnym za tworzenie obiekt�w w grze),
            // aby uruchomi� aktywno�� wulkanu na spawnerze.
            spawner.SetVolcanoActive(true);

            // Ustawienie czasu timera (currentTimer) na warto�� zdefiniowan� w zmiennej timerDuration.
            currentTimer = timerDuration;

            // Rozpocz�cie korutyny (IEnumerator) w celu wy��czenia aktywno�ci wulkanu po zadanym op�nieniu.
            StartCoroutine(DisableVolcanoAfterDelay(5f));
        }
        else
        {
            // Je�li zdolno�� wulkanu jest ju� aktywna, resetujemy czas timera do warto�ci zdefiniowanej w zmiennej timerDuration,
            // co pozwoli przed�u�y� czas aktywno�ci zdolno�ci.
            currentTimer = timerDuration;
        }
    }

    // Funkcja korutyny (IEnumerator), kt�ra wy��cza aktywno�� wulkanu po zadanym op�nieniu.
    private IEnumerator DisableVolcanoAfterDelay(float delay)
    {
        // Poczekanie na op�nienie, kt�re zosta�o przekazane do funkcji.
        yield return new WaitForSeconds(delay);

        // Po up�ywie op�nienia, wy��czamy aktywno�� wulkanu (zmieniaj�c odpowiedni� zmienn� na false)
        // oraz wywo�ujemy metod� SetVolcanoActive(false) na obiekcie spawner, aby wy��czy� aktywno�� wulkanu na spawnerze.
        isVolcanoActive = false;
        spawner.SetVolcanoActive(false);
    }
    public void Freeze()
    {
        if (!isFreezeActive)
        {
            isFreezeActive = true;
            // Ustawienie pozosta�ego czasu trwania zatrzymania na 5 sekund.
            remainingFreezeTime = 5f;
            UpdateFreezeTimeDisplay(); // Wy�wietlamy komunikat "Freeze!" na ekranie.
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
        GameOverScreen.Setup(score);//aktywowanie funkcji setup z skryptu GameOver z przekazaniem do wysweitlania punkt�w 
        //wy��czanie funckji blade i spawner
        //StartCoroutine(ExplodeSequence());//uruchumienie korutyny 
    }
private IEnumerator ExplodeSequence()//Jest to korutyna, kt�ra odpowiada za sekwencj� dzia�a� podczas efektu eksplozji.
    {
        float elapsed = 0f;//zmienna przechowuj�ca up�ywaj�cy czas
        float duration = 0.5f;////zmienna przechowuj�ca czas trwania eksplozji

        while (elapsed < duration) //petla por�wn�j�ca poprzednie zmienne 
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.black, t);
            //Oblicza warto�� t w zakresie od 0 do 1 na podstawie stosunku elapsed do duration. t jest u�ywane jako parametr do funkcji Color.Lerp, kt�ra interpoluje mi�dzy dwoma kolorami: Color.clear (przezroczysty) i Color.black (czarny). Ta interpolacja pozwala na p�ynne przej�cie od przezroczysto�ci do koloru czarnego, co daje efekt zanikania ekranu.
            Time.timeScale = 1f - t;//Zmniejsza Time.timeScale z warto�ci 1 do 1f - t. Time.timeScale kontroluje pr�dko�� czasu w grze. Zmniejszenie go powoduje zwolnienie akcji na scenie, co jest u�yteczne do uzyskania efektu "spowolnienia" lub "zamro�enia" gry w momencie eksplozji.
            elapsed += Time.unscaledDeltaTime;//Zwi�ksza elapsed o czas trwania klatki (Time.unscaledDeltaTime) - wa�ne jest, aby u�ywa� Time.unscaledDeltaTime, aby czas dzia�a� niezale�nie od zmiany warto�ci Time.timeScale

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);
        //Poza p�tl� while w korutynie jest op�nienie za pomoc� yield return new WaitForSecondsRealtime(1f). To op�nienie trwa 1 sekund� w czasie rzeczywistym, co pozwala na zatrzymanie ekranu na chwil� po eksplozji.
        NewGame();//wywo�anie nowej gry 

        elapsed = 0f;//reset up�ywu czasu

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.black, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }//Nast�pnie w drugiej p�tli while, kt�ra jest bardzo podobna do pierwszej, interpolujemy kolor ekranu z powrotem z koloru czarnego do przezroczystego, aby uzyska� efekt powrotu do normalnej rozgrywki.
    }
}
