using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy, IDamageable //W miejsu gdzie jest enemy zazwyczaj jest MonoBehaviour jendak zmieniaj�c to na nasz nowy skrypt o nazwie enemy, teraz ten skrypt
    //dziedziczy po skrypcie enemy, dodatkowy dziedziczymy po interfejsie od obra�e�, zasada jest taka, �e mo�emy dziedziczy� po 
    //WY��CZANIE JEDNEJ klasie, jednak mo�emy dziedziczy� po wybranej ilo�ci interfejs�w
{
    //Dwie linijki poni�ej s� to bazowo usawione zmienne i funkcje, kt�re wymusza nam dziedziczenie po interfejsie
    public int health { get; set; }
  
    public override void Initialization()//U�ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo�na by�o j� personalizowa� pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, �e zanim wywo��a si� nasz unikalny kod najpierw wykona si� kod z skryptu Enemy a potem to co tutaj si� napisze
        health = base.health;//zmienna od zdrowia z interfejsu, kt�rej MUSIMY u�y�, dostaje warto�� zdrowia, z klasy enemy, kt�ra to deklaruje
    }
   public override void Movement()
    {
        base.Movement();
      
    }
    public void Damage()
    {
        if(_isDeath == true)//ma�a logika zapobiegaj�ca temu,�eby diamenty nie wypada�y kilka razy z matrwego przeciwnika
        {
            return;
        }
        health--;//dekrementacja �ycia o jedno gdy wywo�a si� metoda obra�e�
        _anim.SetTrigger("Hit");//Gdy gracz zaatakuje szkieleta wyzwala jego animacje dostania hitka
        isHit = true;
        _anim.SetBool("InCombat", true);//wyzwalamy boola jako true aby po zaatakowaniu szkieleta zosta� on w trybie idle a nie moonwalka robi�
        if (health < 1)//je�eli �ycia si� sko�cz�
        {
            _isDeath = true;
            _anim.SetTrigger("Death");//Gdy �ycia si� sko�cz� odpalamy animacje umierania 
            for(int i =0; i<5; i++ )
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa�nienie diamenciku na pozycji giganta ignotuj�c rotacje

            }
        }
    }
}









/* STARE PODEJ�CIE NIE ZA POMOC� DZIEDZICZ�CYCH METOD VIRTUALNYCH
private Vector3 _CurrentTarget;//zmienna punktowa do trzymania waypoint�w
    private Animator _anim;//zmienna animatora do uchwytu
    private SpriteRenderer _Sprite;//zmienna dla spriterenderara dla uchwytu= 
 * 
public void Start()
    {
        _anim = GetComponentInChildren<Animator>();//do zmiennej anim przypisujemy konponent, kt�ry  nas interesujje z obiektu w grze
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie wymaganego componentu z unity do tej zmiennej
    }
 * 
public override void Update()//nadpisywana abstrakcyjna metoda z zkryptu Enemey, MUSI ona by� override inaczej b�dzie error
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) //je�li obecnie odtwarzan� animacja jest ta o nazie "Idle"(GetCurrent... jest to wbudowana funckja unity do szukania i okre�lenia statnu danej animacji)
        {
            return;//po prostu zwraca nic �eby do ko�ca si� wykona�a
        }   
    }
 * 
void Movement()
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
        transform.position = Vector3.MoveTowards(transform.position, _CurrentTarget, speed * Time.deltaTime);
        //aktualana pozycja giganta jest przypisaywania za pomoc� pozycji z funkcje MoveTowards, kt�ra przyjmuje obecn� pozycje, now� gdzie ma zmierz�c i pr�dko�c poruszania

    }
 */