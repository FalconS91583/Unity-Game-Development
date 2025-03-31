using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour//dodaj¹c abstract jesteœy w stanie u¿ywaæ abstrakcyjnych metod, co umo¿liwia nam...(Opis w metodzie update)
{
    [SerializeField]//linijka ¿eby widzieæ te zmienne w unity
    protected int health;
    [SerializeField]
    protected int speed;
    [SerializeField]
    protected int gems;//protected sprawia, ¿e dla wszystkiego ta zmienna jest prywatna JEDNAK skrypty klasy itp które dziedzicz¹
    //po tej klasie, skrypcie maj¹ j¹ jako publiczne, 
    [SerializeField]
    protected Transform pointA, pointB;//zmienne typu lokacyjnego do waypointów dla botów

    protected Vector3 _CurrentTarget;//zmienna punktowa do trzymania waypointów
    protected Animator _anim;//zmienna animatora do uchwytu
    protected SpriteRenderer _Sprite;//zmienna dla spriterenderara dla uchwytu

    protected bool isHit = false;//zmienna do badania czy wróg zosta³ zaatakowany
    protected Player _player;//zmienna pod uchwyt dla obiektu gracza
    protected bool _isDeath = false;//zmienna badaj¹ca czy wróg umar³

    public GameObject DiamondPrefab;//zmienna do obiektu w grze do diamentów
    public virtual void Initialization()
    {
        _anim = GetComponentInChildren<Animator>();//do zmiennej anim przypisujemy konponent, który  nas interesujje z obiektu w grze
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie wymaganego componentu z unity do tej zmiennej
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//referencja do obiektu gracza
    }
    private void Start()
    {
        Initialization();//Wywo³anie metody inilizacyjnej gdy tylko odpali siê gra
    }
    public virtual void Update()//funckja, która sprawia, ¿e podczas animacji Idle, bot siê zatrzymuje i wykonuje t¹ animacje do koñca
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _anim.GetBool("InCombat") == false) //je¿li obecnie odtwarzan¹ animacja jest ta o nazie "Idle"(GetCurrent... jest to wbudowana funckja unity do szukania i okreœlenia statnu danej animacji)
        {//oraz gdy przeciwnik nie jest w walce
            return;//po prostu zwraca nic ¿eby do koñca siê wykona³a
        }
        if (_isDeath == false)//jak wróg ¿yje 
        {
            Movement();//No to wykonuje funkcje poruszania
        }
        
    }

    public virtual void Movement()//funkcja do poruszania siê botó
    {
        if (_CurrentTarget == pointA.position)//warunek do obracania siê prawid³owego giganta
        {
            _Sprite.flipX = true;
        }
        else
        {
            _Sprite.flipX = false;
        }

        if (transform.position == pointA.position)//je¿eli aktualna pozycja MossGianta jest równa naszemu waypointowi A
        {
            _CurrentTarget = pointB.position;//ustawienie celu jako punktB
            _anim.SetTrigger("Idle");// jak ju¿ sobie dojdzie do celu, to wymuszamy zagranie animacji Idle        
        }
        else if (transform.position == pointB.position)
        {//jezeli jego pozycja jest teraz równa pozycji punktu B
            _CurrentTarget = pointA.position;//ustawienie celu jako punktA
            _anim.SetTrigger("Idle");
        }
        if(isHit == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _CurrentTarget, speed * Time.deltaTime);
            //aktualana pozycja giganta jest przypisaywania za pomoc¹ pozycji z funkcje MoveTowards, która przyjmuje obecn¹ pozycje, now¹ gdzie ma zmierz¹c i prêdkoœc poruszania

        }
        float distance = Vector3.Distance(transform.localPosition, _player.transform.localPosition);//do zmiennej distance przypisujemy odleg³oœc miêdzy 
        //przeciwikiem a graczem
        if(distance > 2.0f)//je¿eli ten dystans jest wiêkszy od 2
        {
            isHit = false; // wy³¹czamy opcje zostania uderzonym 
            _anim.SetBool("InCombat", false);//wy³¹czamy tryb bojowy przeciwnika
        }
        Vector3 direction = _player.transform.localPosition - transform.localPosition;//do zmiennej okreœlaj¹cej zwrot przeciwnika 
        //przypisujemy pozycje gracza minus pozycje przeciwnika, da nam to mo¿liwoœæ, ¿e np gdy ta wartoœc jest na + to 
        //obróci siê w prawo a gdy - to w lewo

        if (direction.x > 0 && _anim.GetBool("InCombat") == true)//je¿eli ta wartoœc wyliczona zwrotu jest wiêksza od 0 i szkielet ma tryb bojowy
        {
            _Sprite.flipX = false;//obracamy szkieleta w strone gracza
        }
        else if (direction.x < 0 && _anim.GetBool("InCombat") == true)
        {
            _Sprite.flipX = true;
        }
    }
    //public abstract void Update();//umo¿³iwia to nam zapisanie takiej funkcji, która od teraz JEST OBOWI¥ZKOWA dla ka¿dego skryptu, który dziedziczy po tym

    /*
     public virtual void attack()//virtual sprawia, ¿e mo¿emy nadpisywaæ t¹ metodê przez jej dziedzicz¹ce, czyli w jednej metodzie
        //mo¿e byæ zachowanie dla ka¿dego wroga ale oni mog¹ j¹ upgredowaæ pod siebie
    {

    }
     * */

}
