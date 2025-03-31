using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//szczeg�owy opis w skrypcie MossGiant
public class Spider : Enemy, IDamageable//dziedziczenie po skrypcie Enemy
{
    public int health { get; set; }
    public GameObject AcidPrefab;//wa�ne przypisa� to w silniku

    public override void Initialization()//U�ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo�na by�o j� personalizowa� pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, �e zanim wywo��a si� nasz unikalny kod najpierw wykona si� kod z skryptu Enemy a potem to co tutaj si� napisze
        health = base.health;//przypisanie warto�ci zdrowai do tej co damy w silniku
    }

    public override void Update()//tylko po to ��by nie by�o error�w w silniku
    {
      
    }
    public void Damage()
    {
        if (_isDeath == true)//ma�a logika zapobiegaj�ca temu,�eby diamenty nie wypada�y kilka razy z matrwego przeciwnika
        {
            return;
        }
        health--;
        if (health < 1)
        {
            _isDeath = true;
            _anim.SetTrigger("Death");//Gdy �ycia si� sko�cz� odpalamy animacje umierania 
            for (int i = 0; i < 3; i++)
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa�nienie diamenciku na pozycji giganta ignotuj�c rotacje

            }
        }
    }
    public override void Movement()//wywo�ujemy metode Movement z opcj� nadpisu
    {
       //nie wywo�ujmy bazowego dzia�ania metody, bo animacja paj�ka si� psuje, ta metoda jest po to aby sta� w miejsu podczs animacji
    }

    public void Attack()
    {
        Instantiate(AcidPrefab, transform.position, Quaternion.identity);//zespa�nienie prefabu kwasu, na pozycji paj�ka, ignoruj�c rotacje
    }
    /*
    public override void attack()//U�ywamy klasy z Enemy o nazwie Attack, z dopiskiem override aby mo�na by�o j� personalizowa� pod tego paj�ka
    {
        base.attack();//powoduje, �e zanim wywo��a si� nasz unikalny kod najpier wykona si� kod z skryptu Enemy a potem to co tutaj si� napisze
    }
     */
}
