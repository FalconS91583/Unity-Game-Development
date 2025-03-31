using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Najwiecej rzeczy opisanych w skrypcie MossGiant

public class Skeleton : Enemy, IDamageable//dziedziczenie po skrypcie Enemy
{
    public int health { get; set; }

    public override void Movement()//nadpisujemy metodê Movement z klasy Enemy
    {
        base.Movement();//najpierw wywo³ujemy to co ma tam bazowo aby siê wykonwa³o
             
    }

    public void Damage()
    {
        if (_isDeath == true)//ma³a logika zapobiegaj¹ca temu,¿eby diamenty nie wypada³y kilka razy z matrwego przeciwnika
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
            for (int i = 0; i < 2; i++)
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa³nienie diamenciku na pozycji giganta ignotuj¹c rotacje

            }
        }
    }
    public override void Initialization()//U¿ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo¿na by³o j¹ personalizowaæ pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, ¿e zanim wywo¹³a siê nasz unikalny kod najpierw wykona siê kod z skryptu Enemy a potem to co tutaj siê napisze
        health = base.health;
    }
}
