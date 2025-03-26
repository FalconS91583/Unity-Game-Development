using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;//zmienna przechowuj�ca referencj� do g��wnej kamery
    private Collider bladeCollider;//zmienna przechowuj�ca referencj� do komponentu Collider przypisanego do tego obiektu skryptu. Collider pozwala na wykrywanie kolizji z innymi obiektami.
    private TrailRenderer bladeTrail;//zmienna przechowuj�ca referencj� do komponentu TrailRenderer przypisanego do tego obiektu skryptu. TrailRenderer pozwala na tworzenie "�ladu" za obiektem w ruchu
    private bool slicing;// zmienna logiczna, kt�ra okre�la, czy obecnie obiekt jest w trakcie ci�cia 

    public Vector3 direction {  get; private set; }//zmienna pozycyjna tr�jwymiarowa przechowuj�ca kierunek ci�cia
    public float sliceForce = 5f;//zmienna przechowuj�ca z jak� si�� obiekt zostanie odepchniety po ci�ciu
    public float minSliceVelocity = 0.01f;//zmianna minimalnej predko�ci myszy do ci�cia 

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }//gdy skrypt jest aktywny przypisujemy referencje do danych komponent�w 

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
        if (Input.GetMouseButtonDown(0))//sprawdzamy czy gracz nacisn�� lewy przycisk myszy zaczynamy ci�cie
        {
            StartSlicing();
        } else if (Input.GetMouseButtonUp(0))//gdy pu�ci przycisk stopujemy ci�cie 
        {
            StopSlicing();
        } else if (slicing)//gdy zmienna slicing jest ustawiona na true czyli jest w terakcie ci�cia to wtedy kontynooujemy 
        {
            ContinuseSlicing();
        }
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);//pobranie pozycji myszki i przeliczenie jej na przestrze� w grze 
        newPosition.z = 0f; // nowa pozycha obiektu

        transform.position = newPosition;
        //w��czenie ci�cia bollidera i �ladu no�a
        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear(); //funkcja do czyszczenia po czasie �ladu noza 
    }

    private void StopSlicing()//zatrzymujemy wszytkie akcje zwi�zane z ci�ciem 
    {
        slicing = false;
        bladeCollider.enabled=false;
        bladeTrail.enabled=false;
    }

    private void ContinuseSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);//pobranie pozycji myszki i przeliczenie jej na przestrze� w grze 
        newPosition.z = 0f;

        direction = newPosition - transform.position;//obliczenie kierunku ci�cia za pomoc� roznicy miedzy pierwotna a nowa pozycja

        float velocity = direction.magnitude / Time.deltaTime;//obliczanie predkosci ciecia dzil�c d�ugo�� kierunku przez czas do ostatniej klatki 
        bladeCollider.enabled = velocity > minSliceVelocity;//w�aczamy collider jezeli pr�dko�� ci�cia jest wieksza niz minimalna wartosc predko��i 

        transform.position = newPosition;//ustawienie nowej pozycji obiektu na myszke 
    }
}
