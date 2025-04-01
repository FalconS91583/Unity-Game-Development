using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//biblioteka dla sztucznej inteligencji obiektów w unity

public class Player : MonoBehaviour {

	private NavMeshAgent _agent;//zmienna zeby postać szła tam gdie gracz kliknie
	private Animator _anim;//zmienna do trzymania dostępu do animatora czyli dla animacji
	private Vector3 _target;//zmienna dla celu podrózy
	public GameObject coinPrefab; // zmienna dla przechowywnia obiedku gry pieniomszaka
	public AudioClip coinSoundEffect; // zmienna do przechowywania audio rzuciania monetki

	private bool _cointToss;//zmienna do przechowywania czy moenta zostałą wykorzystana przez gracza
	// Use this for initialization
	void Start () {
		_agent = GetComponent<NavMeshAgent>();//przypisanie do zmienej pobranego komponentu do poruszania się navmesh
		_anim = GetComponentInChildren<Animator>();//pobranie dostępu do komponentu animator musimy inchildren, bo animator jest w postaci Darren a Darren jest w Player więc darren jest dzieckiem Player i stamtąd trzeba go pobrać 
	}
	
	// Update is called once per frame
	void Update () {
		//Po kliknieciu lewej myszki pobieranie pozycji gdzie to się klikło i poruszyć się tam(Logika z poruszania się z LOLa)
		//if bo sprawdza czy sie klikło 
		if(Input.GetMouseButtonDown(0))//0 dla lewy przycisk myszy 1 dla kółka 2 dla prawego albo na odwrót 
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);//Stworzernie jakiegos ray i danie temu wartości kamery gdzie się klikło koordynaty 
			RaycastHit hitInfo; // Zmienna do przechowywania w co się jebło
			
			if(Physics.Raycast(rayOrigin, out hitInfo))// w cokoleik sie jebnie to zostanie to zapisane w hitinfo 
			{
				Debug.Log("Hit: " + hitInfo.point);
				_agent.SetDestination(hitInfo.point);//dla naszej zmiennej danie opcji setdestination zeby w momencie gdy dostanie wspołrzędne z getinfo to sie tam przejdzie(sam wybierze sobie ścieżke poruszy się tam ruch botów itp) Na raize wchodzi w wszystkie elementy otoczenia i przenika przez nie
				_anim.SetBool("Walk", true);//ustawiamy naszą zmienną która ma animator, ze nasza animacja podpisane Walk wykona się w tym ifie jak się spełni warunek
				_target = hitInfo.point;//przypisanie zmiennej docelowej(target) pozydji hitinfo(jest to po to aby uzywac lokalizacji hit info poza tym ifem, bo zmienna jest zdeklatrowana tylko w tym fie
				//warto wiedzieć
				//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); //stworzenie kostki która spawni się zależnie gdzie klieknie się coś jak to zielone gówno z lola żeby wiedzieć gdzie się klikło i gdzie się idzie
				//cube.transform.position = hitInfo.point; //spawn kostki na pozycji która jest zapisana w hitinfo
				//Destroy(cube, 0.5f);//zniszczenie znacznika(kostki) po jakimś czasie
			}
		}
		float distance = Vector3.Distance(transform.position, _target);//nowa zmienna distance, która bada odległośc miedzy tymi dwona innymi zmiennymi transform.position to gracz _target to koordynaty z hitinfo gdzie sie idzie
		if(distance < 1)//jak obliczony dystans miedzy tymi odległościami jest mniejszy od jeden 
		{
			_anim.SetBool("Walk", false);// wyłączamy animacje
		}

		if(Input.GetMouseButtonDown(1) && _cointToss == false) // jeżeli zostanie wcisniety prawy przycisk myszy oraz zmienna przechowująca stan rzucenia monetą jest false
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);//Stworzernie jakiegos ray i danie temu wartości kamery gdzie się klikło koordynaty 
			RaycastHit hitInfo; //tutaj są zapamiętane i przechowywane kliknięcia w tym przypadku prawego przycisku myszy
			
			if (Physics.Raycast(rayOrigin, out hitInfo))// zapisanie w hit info wszystkiego
			{
				_anim.SetTrigger("Throw");//w animatorze został stworzony trigger, który dzieki tej funkcji Unity uruchamiamy kiedy nam się chce poprzez nazwe tej animacji
				_cointToss = true;//jeżeli klineło się prawy przycisk  myszy zmienna staje się true czyli blokuje możliwośc rzucenia wiekszą liczbą moenat
				Instantiate(coinPrefab, hitInfo.point, Quaternion.identity);//zrespałnienie prefabu monetki na danej pozycji, nie przemowanie się rotacja
                AudioSource.PlayClipAtPoint(coinSoundEffect, transform.position);//funkcja unity do odpalenia audio dżwieku przyjmuje zmienna dżwieku i pozycje zagrania
				SentAiToCoin(hitInfo.point);//wywołanie funkcji do przemieszczenia się strażników, która podaje jej koordynaty miejsca rzucenia monetka
            }
        }
	}

	void SentAiToCoin(Vector3 coinPos) // nowa funckja do przemieszczenia się strażników do miejsca gdzie zsotała użyta moneta,przyjmuje wartośc jakiejs pozycji
	{
		GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard1");//Tablica obiektów do której są przypisane wszystie obiekty gry z danym tagiem w tym przypadku 3 strażników
		foreach(var guard in guards) // pętala która dla każdego strażnika w tablicy strażników(guard to może być wszystko x czy cos, a var to po prostu typ obiektu gry, też może być inny)
		{
			NavMeshAgent currentAgent = guard.GetComponent<NavMeshAgent>();//nowa zmienna navmash(Pole po którym się poruszają strażnicy) i do zmiennej currentGuard przypisywany jest wzięcie komponentu navmash od tego danego strażnika, czyli dla 1 strazniak to będzie miało jego  komponent jak skończy to bierzze drugiego itd
			GuardAI currentGuard = guard.GetComponent<GuardAI>(); //handle do skrytu botów, zmienna do przechowywania innego skryptu
			Animator currentAnim = guard.GetComponent<Animator>();//handle dla komponentu animacji botów aby można było nim tutaj zarządzać
			currentGuard.coinToss = true;//ustawienie zmiennej coinToss w skrypcie GuardAI gdy będzie wykonywał sie ten if
			currentAgent.SetDestination(coinPos);//Kiedy już mamy dostep do jego pola porusznia się dajemy temu strażnikowy nowy cel podrózy czyli pozycje monetki
			currentAnim.SetBool("Walk", true);//uruchomienie animacji chodzenia dla strażników w moemencie podchodzenia do monetki
			currentGuard.coinPos = coinPos;//przypisanie pozycji monety z tego skryptu do skryptu strażników
		}
	}
}
