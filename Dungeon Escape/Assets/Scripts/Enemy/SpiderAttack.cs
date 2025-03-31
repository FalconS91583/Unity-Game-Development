using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private Spider _spider;//zmienna(uchwyt) pod skrypt spider
    public void Start()
    {
        _spider = transform.parent.GetComponent<Spider>();//przypisanie pod uchwyt skreyptu paj�ka
    }

    public void Fire()
    {
        _spider.Attack();//Wywo�anie metody atak z skryptu paj�ka
    }
}
