using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour//dodaj�c abstract jeste�y w stanie u�ywa� abstrakcyjnych metod, co umo�liwia nam...(Opis w metodzie update)
{
    [SerializeField]//linijka �eby widzie� te zmienne w unity
    protected int health;
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected int gems;//protected sprawia, �e dla wszystkiego ta zmienna jest prywatna JEDNAK skrypty klasy itp kt�re dziedzicz�
    //po tej klasie, skrypcie maj� j� jako publiczne, 
    [SerializeField]
    protected Transform pointA, pointB;//zmienne typu lokacyjnego do waypoint�w dla bot�w

    protected Vector3 _CurrentTarget;//zmienna punktowa do trzymania waypoint�w
    protected Animator _anim;//zmienna animatora do uchwytu
    protected SpriteRenderer _Sprite;//zmienna dla spriterenderara dla uchwytu

    protected bool isHit = false;//zmienna do badania czy wr�g zosta� zaatakowany
    protected Player _player;//zmienna pod uchwyt dla obiektu gracza
    protected bool _isDeath = false;//zmienna badaj�ca czy wr�g umar�

    public GameObject DiamondPrefab;//zmienna do obiektu w grze do diament�w
    public virtual void Initialization()
    {
        _anim = GetComponentInChildren<Animator>();//do zmiennej anim przypisujemy konponent, kt�ry  nas interesujje z obiektu w grze
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie wymaganego componentu z unity do tej zmiennej
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//referencja do obiektu gracza
    }
    private void Start()
    {
        Initialization();//Wywo�anie metody inilizacyjnej gdy tylko odpali si� gra
    }
    public virtual void Update()//funckja, kt�ra sprawia, �e podczas animacji Idle, bot si� zatrzymuje i wykonuje t� animacje do ko�ca
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _anim.GetBool("InCombat") == false) //je�li obecnie odtwarzan� animacja jest ta o nazie "Idle"(GetCurrent... jest to wbudowana funckja unity do szukania i okre�lenia statnu danej animacji)
        {//oraz gdy przeciwnik nie jest w walce
            return;//po prostu zwraca nic �eby do ko�ca si� wykona�a
        }
        if (_isDeath == false)//jak wr�g �yje 
        {
            Movement();//No to wykonuje funkcje poruszania
        }
        
    }

    public virtual void Movement()//funkcja do poruszania si� bot�
    {
        if (_CurrentTarget == pointA.position)//warunek do obracania si� prawid�owego giganta
        {
            _Sprite.flipX = true;
        }
        else
        {
            _Sprite.flipX = false;
        }

        if (transform.position == pointA.position)//je�eli aktualna pozycja MossGianta jest r�wna naszemu waypointowi A
        {
            _CurrentTarget = pointB.position;//ustawienie celu jako punktB
            _anim.SetTrigger("Idle");// jak ju� sobie dojdzie do celu, to wymuszamy zagranie animacji Idle        
        }
        else if (transform.position == pointB.position)
        {//jezeli jego pozycja jest teraz r�wna pozycji punktu B
            _CurrentTarget = pointA.position;//ustawienie celu jako punktA
            _anim.SetTrigger("Idle");
        }
        if(isHit == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _CurrentTarget, speed * Time.deltaTime);
            //aktualana pozycja giganta jest przypisaywania za pomoc� pozycji z funkcje MoveTowards, kt�ra przyjmuje obecn� pozycje, now� gdzie ma zmierz�c i pr�dko�c poruszania

        }
        float distance = Vector3.Distance(transform.localPosition, _player.transform.localPosition);//do zmiennej distance przypisujemy odleg�o�c mi�dzy 
        //przeciwikiem a graczem
        if(distance > 2.0f)//je�eli ten dystans jest wi�kszy od 2
        {
            isHit = false; // wy��czamy opcje zostania uderzonym 
            _anim.SetBool("InCombat", false);//wy��czamy tryb bojowy przeciwnika
        }
        Vector3 direction = _player.transform.localPosition - transform.localPosition;//do zmiennej okre�laj�cej zwrot przeciwnika 
        //przypisujemy pozycje gracza minus pozycje przeciwnika, da nam to mo�liwo��, �e np gdy ta warto�c jest na + to 
        //obr�ci si� w prawo a gdy - to w lewo

        if (direction.x > 0 && _anim.GetBool("InCombat") == true)//je�eli ta warto�c wyliczona zwrotu jest wi�ksza od 0 i szkielet ma tryb bojowy
        {
            _Sprite.flipX = false;//obracamy szkieleta w strone gracza
        }
        else if (direction.x < 0 && _anim.GetBool("InCombat") == true)
        {
            _Sprite.flipX = true;
        }
    }
    //public abstract void Update();//umo��iwia to nam zapisanie takiej funkcji, kt�ra od teraz JEST OBOWI�ZKOWA dla ka�dego skryptu, kt�ry dziedziczy po tym

    /*
     public virtual void attack()//virtual sprawia, �e mo�emy nadpisywa� t� metod� przez jej dziedzicz�ce, czyli w jednej metodzie
        //mo�e by� zachowanie dla ka�dego wroga ale oni mog� j� upgredowa� pod siebie
    {

    }
     * */

}
