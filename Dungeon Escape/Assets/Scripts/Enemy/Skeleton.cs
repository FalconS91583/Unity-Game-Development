using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Najwiecej rzeczy opisanych w skrypcie MossGiant

public class Skeleton : Enemy, IDamageable//dziedziczenie po skrypcie Enemy
{
    public int health { get; set; }

    public override void Movement()//nadpisujemy metod� Movement z klasy Enemy
    {
        base.Movement();//najpierw wywo�ujemy to co ma tam bazowo aby si� wykonwa�o
             
    }

    public void Damage()
    {
        if (_isDeath == true)//ma�a logika zapobiegaj�ca temu,�eby diamenty nie wypada�y kilka razy z matrwego przeciwnika
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
            for (int i = 0; i < 2; i++)
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa�nienie diamenciku na pozycji giganta ignotuj�c rotacje

            }
        }
    }
    public override void Initialization()//U�ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo�na by�o j� personalizowa� pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, �e zanim wywo��a si� nasz unikalny kod najpierw wykona si� kod z skryptu Enemy a potem to co tutaj si� napisze
        health = base.health;
    }
}
