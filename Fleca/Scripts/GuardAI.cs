using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour {

    public List<Transform> wayPoints; //lista potencjalnych waypointów poruszania sie strazników(lista jest dynamiczna mozna zmieniać rzeczy w czasie rzeczywistym) tablica nie
    public bool coinToss;//zmienna do przechowywania czy moneta została rzucana przez gracza
    public Vector3 coinPos; //zmienna do przechowywania pozycji monetki
    private NavMeshAgent _agent; // zmienna do poruszania się postaci po tym navmesh
    [SerializeField]
    private int currentTarget;//zmienna która sobie będzie przechowywała numer waypointa
    private bool reverse = false; //zmienna, która będzie służyła do ruszania się bota na false do ABC a na true do CBA 
    private bool _targetReached = false;//zmienna przechowująca stan czy dany cel jest osiągniety, jest to po to aby dodać realizmu do bota zeby nie latał jak pojeb prawo lewo tylko zatrzymał się na chwile
    private Animator _anim; // zmienna do komponentu z animacjami
    void Start () {

		_agent = GetComponent<NavMeshAgent>();//uchwyt przypisujący do tej zmiennej komponent navmash agent zeby móc z niego korzystac i wgl ingerować z nim
        _anim = GetComponent<Animator>();//uchwyt przypisujący do tej zmiennej komponent animator z unity i teraz można kodować ten komponent unity

    }
    void Update () {

        if(wayPoints.Count > 0 && wayPoints[currentTarget] != null && coinToss == false) //jezeli liczba waypointów jest wieksza od 0 oraz czy dany waypoint w ogóle istnieje dodatkowo czy zmienna coinToss jest false
        {
            _agent.SetDestination(wayPoints[currentTarget].position);//przemieszczenie się bota do danego waypointa
            

            float distance = Vector3.Distance(transform.position, wayPoints[currentTarget].position);//zmienna do przechowywania odległości między danymi punktami(pozycją oraz waypointem
            if (distance < 1 && (currentTarget == 0 || currentTarget == wayPoints.Count-1))//logika włączająca i wyłaczająca animacje chodzenia zalężnie od danego waypoint i stanu bota
            {
                if (_anim != null)//if do sprawdzania czy wgl ten komponent istnieje przydatne dla dobrego nawyku unikania błędów
                {
                    _anim.SetBool("Walk", false);//Wyłączenie animacji chodzenia i sobie będzie stał
                }
                
            } else
            {
                if (_anim != null)
                {
                    _anim.SetBool("Walk", true);//doaplenie animacji chodzenia
                }
                
            }

            if (distance < 1.0f && _targetReached == false)//sprawdzenie dystansu miedzy celem bota a botem //jezeli cel jest mniejszy od 1 oraz zmienna osiągniecia celu jest false
            {
                if(wayPoints.Count < 2)
                {
                    return;
                        
                }
                if ((currentTarget == 0 || currentTarget == wayPoints.Count -1) && wayPoints.Count > 1)
                {
                    _targetReached = true;//usatawienie zmiennej do osiagniecia celu na true

                    StartCoroutine(WaitBeforeMoving());//wywołanie ienumatora trzymającego logike poruszania sie botów
                }
                else //wszelka logika tutaj jest zapobieganiem stopownia sie bota w środkowym punkcie oraz zapobieganiuem aby waypointy nie wyszły poza zakres
                {
                    if(reverse == true)
                    {
                        currentTarget--;
                        if(currentTarget <= 0)
                        {
                            reverse = false;
                            currentTarget = 0;
                        }
                    }else
                    {
                        currentTarget++;
                    }
                }
            }
        }
        else// w przeciwnym wypadku czyli gdy moneta została rzucona
        {
            float distance = Vector3.Distance(transform.position, coinPos);//zmienna przechowująca dystans między strąznikami a monetką
            
            if(distance < 4f)// jeżeli ten dystans jest mniejszy od 1
            {
                _anim.SetBool("Walk", false); // wtedy animacja chodzenia się wyłącza
            }
        }
    }

    IEnumerator WaitBeforeMoving()//ienumerator dla stowrzenia logiki zatrzymania się na jakiś czas i odczekaniu iles tam sekund
    {
        //logika dla tego aby w momencie gdy jest bot w 1 celu i otstanim zatrzymał się na 2 sekundy a wszędzie pomiedzy szedł bez przerw
        if(currentTarget == 0)
        {
            yield return new WaitForSeconds(Random.Range(2,6));
        }else if(currentTarget == wayPoints.Count -1 )
        {
            yield return new WaitForSeconds(Random.Range(2, 6));
        }   


        if(reverse == true)
        {
            currentTarget--; //wtedy dekrementacja, żeby szedł do ,,starego" waypointa
            if (currentTarget == 0) // jeżeli juz wrócił z CBA to A jest 0 sprawdza czy już tam jest 
            {
                reverse = false;//jak tak daje dekrementacje na false
                currentTarget = 0;//i ustawia go na zero żeby na nowo mógł iść do ABC
            }

        } else if (reverse == false)
        {
            currentTarget++;//wtedy inkrementacja, żeby szedł do  nowego waypointa
            if (currentTarget == wayPoints.Count)// każdorazowe sprawdzanie czy dotarliśmy do końca waypointów
            {
                reverse = true; // jeżeli tak ustawiamy reverse na true
                currentTarget--;//i dekrementujemy aktualną pozycje zeby zamiast ABC szedł do CBA
            }
        }
        _targetReached = false;//po 2 sekundach zwalniamy program i działa dalej
    }
}
/* STARE ROZWIAZANIE
if (wayPoints.Count > 0)//dajemy to po to, bo jeden straznik sie nie rusza i ma miec wartośc w liście 0, a bez tego jest error w grze, bo daliśmy w dtugim ife sprawdzenie dotycząte tego więc takie naprawienie	
{
    if (wayPoints[0] != null)//sprawdzamy czy ten pierwszy waypoint nie jest pusty, jeżeli nie jest pusty wetedy dzieje się przypisanie
    {
        currentTarget = wayPoints[0];//przypisanie ustawieniu bota do pierwszego waypointa
        _agent.SetDestination(currentTarget.position);//ustawienie punktu docelowego na nasz aktualny cel, czyli
                                                      //nasz currenttarger po odpaleniu jest na miejsu startowy mstraznika a potem tym setdestination ustawiamy, zeby szedł sobie
                                                      //do punktuA, który jest w edytorze, jest on przypisany do listy utworzonej a sama lista zapełnina jest tymi punktami w strażniku
                                                      //pdsumowując Lista Waypoint[0] to tam gdzie strażnik sobie stoi, on ma sktypt z tą listą, a w niej są 2 punkty A i B
                                                      //dzieki temu setDestination i tej liście w skrypcie po odpaleniu on sobie będzie wybierał drogę od tego punktu A i sam se tam pójdzie
                                                      //jednak on sobie wybiera najszybszą drogę wieć moze iść inaczej niż chcemy wtedy bedzie trzbea dac więcej waypointów 
                                                      //zebt przechodził przez każden jak nam się chce
    }
}
if (wayPoints.Count > 0)//dajemy to po to, bo jeden straznik sie nie rusza i ma miec wartośc w liście 0, a bez tego jest error w grze, bo daliśmy w dtugim ife sprawdzenie dotycząte tego więc takie naprawienie	
{
    if (wayPoints[0] != null)//sprawdzamy czy ten pierwszy waypoint nie jest pusty, jeżeli nie jest pusty wetedy dzieje się przypisanie
    {
        currentTarget = wayPoints[0];//przypisanie ustawieniu bota do pierwszego waypointa
        _agent.SetDestination(currentTarget.position);//ustawienie punktu docelowego na nasz aktualny cel, czyli
                                                      //nasz currenttarger po odpaleniu jest na miejsu startowy mstraznika a potem tym setdestination ustawiamy, zeby szedł sobie
                                                      //do punktuA, który jest w edytorze, jest on przypisany do listy utworzonej a sama lista zapełnina jest tymi punktami w strażniku
                                                      //pdsumowując Lista Waypoint[0] to tam gdzie strażnik sobie stoi, on ma sktypt z tą listą, a w niej są 2 punkty A i B
                                                      //dzieki temu setDestination i tej liście w skrypcie po odpaleniu on sobie będzie wybierał drogę od tego punktu A i sam se tam pójdzie
                                                      //jednak on sobie wybiera najszybszą drogę wieć moze iść inaczej niż chcemy wtedy bedzie trzbea dac więcej waypointów 
                                                      //zebt przechodził przez każden jak nam się chce
    }
}
*/