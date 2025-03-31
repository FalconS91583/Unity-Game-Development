using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy, IDamageable //W miejsu gdzie jest enemy zazwyczaj jest MonoBehaviour jendak zmieniaj¹c to na nasz nowy skrypt o nazwie enemy, teraz ten skrypt
    //dziedziczy po skrypcie enemy, dodatkowy dziedziczymy po interfejsie od obra¿eñ, zasada jest taka, ¿e mo¿emy dziedziczyæ po 
    //WY£¥CZANIE JEDNEJ klasie, jednak mo¿emy dziedziczyæ po wybranej iloœci interfejsów
{
    //Dwie linijki poni¿ej s¹ to bazowo usawione zmienne i funkcje, które wymusza nam dziedziczenie po interfejsie
    public int health { get; set; }
  
    public override void Initialization()//U¿ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo¿na by³o j¹ personalizowaæ pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, ¿e zanim wywo¹³a siê nasz unikalny kod najpierw wykona siê kod z skryptu Enemy a potem to co tutaj siê napisze
        health = base.health;//zmienna od zdrowia z interfejsu, której MUSIMY u¿yæ, dostaje wartoœæ zdrowia, z klasy enemy, która to deklaruje
    }
   public override void Movement()
    {
        base.Movement();
      
    }
    public void Damage()
    {
        if(_isDeath == true)//ma³a logika zapobiegaj¹ca temu,¿eby diamenty nie wypada³y kilka razy z matrwego przeciwnika
        {
            return;
        }
        health--;//dekrementacja ¿ycia o jedno gdy wywo³a siê metoda obra¿eñ
        _anim.SetTrigger("Hit");//Gdy gracz zaatakuje szkieleta wyzwala jego animacje dostania hitka
        isHit = true;
        _anim.SetBool("InCombat", true);//wyzwalamy boola jako true aby po zaatakowaniu szkieleta zosta³ on w trybie idle a nie moonwalka robi³
        if (health < 1)//je¿eli ¿ycia siê skoñcz¹
        {
            _isDeath = true;
            _anim.SetTrigger("Death");//Gdy ¿ycia siê skoñcz¹ odpalamy animacje umierania 
            for(int i =0; i<5; i++ )
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa³nienie diamenciku na pozycji giganta ignotuj¹c rotacje

            }
        }
    }
}









/* STARE PODEJŒCIE NIE ZA POMOC¥ DZIEDZICZ¥CYCH METOD VIRTUALNYCH
private Vector3 _CurrentTarget;//zmienna punktowa do trzymania waypointów
    private Animator _anim;//zmienna animatora do uchwytu
    private SpriteRenderer _Sprite;//zmienna dla spriterenderara dla uchwytu= 
 * 
public void Start()
    {
        _anim = GetComponentInChildren<Animator>();//do zmiennej anim przypisujemy konponent, który  nas interesujje z obiektu w grze
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie wymaganego componentu z unity do tej zmiennej
    }
 * 
public override void Update()//nadpisywana abstrakcyjna metoda z zkryptu Enemey, MUSI ona byæ override inaczej bêdzie error
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) //je¿li obecnie odtwarzan¹ animacja jest ta o nazie "Idle"(GetCurrent... jest to wbudowana funckja unity do szukania i okreœlenia statnu danej animacji)
        {
            return;//po prostu zwraca nic ¿eby do koñca siê wykona³a
        }   
    }
 * 
void Movement()
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
        transform.position = Vector3.MoveTowards(transform.position, _CurrentTarget, speed * Time.deltaTime);
        //aktualana pozycja giganta jest przypisaywania za pomoc¹ pozycji z funkcje MoveTowards, która przyjmuje obecn¹ pozycje, now¹ gdzie ma zmierz¹c i prêdkoœc poruszania

    }
 */