using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private Spider _spider;//zmienna(uchwyt) pod skrypt spider
    public void Start()
    {
        _spider = transform.parent.GetComponent<Spider>();//przypisanie pod uchwyt skreyptu paj¹ka
    }

    public void Fire()
    {
        _spider.Attack();//Wywo³anie metody atak z skryptu paj¹ka
    }
}
