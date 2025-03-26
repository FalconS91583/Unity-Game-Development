using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;//zmienna przechowuj¹ca referencjê do g³ównej kamery
    private Collider bladeCollider;//zmienna przechowuj¹ca referencjê do komponentu Collider przypisanego do tego obiektu skryptu. Collider pozwala na wykrywanie kolizji z innymi obiektami.
    private TrailRenderer bladeTrail;//zmienna przechowuj¹ca referencjê do komponentu TrailRenderer przypisanego do tego obiektu skryptu. TrailRenderer pozwala na tworzenie "œladu" za obiektem w ruchu
    private bool slicing;// zmienna logiczna, która okreœla, czy obecnie obiekt jest w trakcie ciêcia 

    public Vector3 direction {  get; private set; }//zmienna pozycyjna trójwymiarowa przechowuj¹ca kierunek ciêcia
    public float sliceForce = 5f;//zmienna przechowuj¹ca z jak¹ si³¹ obiekt zostanie odepchniety po ciêciu
    public float minSliceVelocity = 0.01f;//zmianna minimalnej predkoœci myszy do ciêcia 

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }//gdy skrypt jest aktywny przypisujemy referencje do danych komponentów 

    private void OnEnable()
    {
        StopSlicing();
    }
    private void OnDisable()
    {
        StopSlicing();
    }
    private void Update()//funckcja upadte wykonuje sie co kaltke w grze 
    {
        if (Input.GetMouseButtonDown(0))//sprawdzamy czy gracz nacisn¹³ lewy przycisk myszy zaczynamy ciêcie
        {
            StartSlicing();
        } else if (Input.GetMouseButtonUp(0))//gdy puœci przycisk stopujemy ciêcie 
        {
            StopSlicing();
        } else if (slicing)//gdy zmienna slicing jest ustawiona na true czyli jest w terakcie ciêcia to wtedy kontynooujemy 
        {
            ContinuseSlicing();
        }
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);//pobranie pozycji myszki i przeliczenie jej na przestrzeñ w grze 
        newPosition.z = 0f; // nowa pozycha obiektu

        transform.position = newPosition;
        //w³¹czenie ciêcia bollidera i œladu no¿a
        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear(); //funkcja do czyszczenia po czasie œladu noza 
    }

    private void StopSlicing()//zatrzymujemy wszytkie akcje zwi¹zane z ciêciem 
    {
        slicing = false;
        bladeCollider.enabled=false;
        bladeTrail.enabled=false;
    }

    private void ContinuseSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);//pobranie pozycji myszki i przeliczenie jej na przestrzeñ w grze 
        newPosition.z = 0f;

        direction = newPosition - transform.position;//obliczenie kierunku ciêcia za pomoc¹ roznicy miedzy pierwotna a nowa pozycja

        float velocity = direction.magnitude / Time.deltaTime;//obliczanie predkosci ciecia dzil¹c d³ugoœæ kierunku przez czas do ostatniej klatki 
        bladeCollider.enabled = velocity > minSliceVelocity;//w³aczamy collider jezeli prêdkoœæ ciêcia jest wieksza niz minimalna wartosc predkoœæi 

        transform.position = newPosition;//ustawienie nowej pozycji obiektu na myszke 
    }
}
