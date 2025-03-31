using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//szczegó³owy opis w skrypcie MossGiant
public class Spider : Enemy, IDamageable//dziedziczenie po skrypcie Enemy
{
    public int health { get; set; }
    public GameObject AcidPrefab;//wa¿ne przypisaæ to w silniku

    public override void Initialization()//U¿ywamy klasy z Enemy o nazwie Init..., z dopiskiem override aby mo¿na by³o j¹ personalizowaæ pod tego przeciwnika konkretngo
    {
        base.Initialization();//powoduje, ¿e zanim wywo¹³a siê nasz unikalny kod najpierw wykona siê kod z skryptu Enemy a potem to co tutaj siê napisze
        health = base.health;//przypisanie wartoœci zdrowai do tej co damy w silniku
    }

    public override void Update()//tylko po to ¿êby nie by³o errorów w silniku
    {
      
    }
    public void Damage()
    {
        if (_isDeath == true)//ma³a logika zapobiegaj¹ca temu,¿eby diamenty nie wypada³y kilka razy z matrwego przeciwnika
        {
            return;
        }
        health--;
        if (health < 1)
        {
            _isDeath = true;
            _anim.SetTrigger("Death");//Gdy ¿ycia siê skoñcz¹ odpalamy animacje umierania 
            for (int i = 0; i < 3; i++)
            {
                Instantiate(DiamondPrefab, transform.position, Quaternion.identity);//zespa³nienie diamenciku na pozycji giganta ignotuj¹c rotacje

            }
        }
    }
    public override void Movement()//wywo³ujemy metode Movement z opcj¹ nadpisu
    {
       //nie wywo³ujmy bazowego dzia³ania metody, bo animacja paj¹ka siê psuje, ta metoda jest po to aby sta³ w miejsu podczs animacji
    }

    public void Attack()
    {
        Instantiate(AcidPrefab, transform.position, Quaternion.identity);//zespa³nienie prefabu kwasu, na pozycji paj¹ka, ignoruj¹c rotacje
    }
    /*
    public override void attack()//U¿ywamy klasy z Enemy o nazwie Attack, z dopiskiem override aby mo¿na by³o j¹ personalizowaæ pod tego paj¹ka
    {
        base.attack();//powoduje, ¿e zanim wywo¹³a siê nasz unikalny kod najpier wykona siê kod z skryptu Enemy a potem to co tutaj siê napisze
    }
     */
}
