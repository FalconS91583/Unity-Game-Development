using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _anim;//uchwyt w skrypcie do animatoira 
    private Animator _SwordAnim;//uchwyt dla animatora ale dla animacji machania miecza
    
    
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();//przypisanie do uchwytu componentu animatora, kt�ry jest  obiekterm dziecka Player, bo on jest w Sprite wie� trzeba go wyszukac po dizecieym obieckie
        _SwordAnim = transform.GetChild(1).GetComponent<Animator>();//jest to przypisanie ichywtu ale w chierarchiczny spos�b, poniewa� animacja miecza jest drugim elementem Gracza wi�c trzeba przej�c przez pierwszy obiekt i potem dosta� si� do miecz 
        //dlatego inna formu�a przypisania uchywtu
    }

    public void Move(float move) //funckja do badania warto�ci float z player skryptu przyjmuj�ca w�a�nie t� warto�c stamt�d
    {
        _anim.SetFloat("Move", Mathf.Abs(move));//Wywo�anie z animatora odpalenia Animacji o nazwie Move, na podstawie warto�ci zmiennej move
        //rozszerzonej o funckje matematyczna warto�� bezwzledna, bo klikanja� D warto�� move jest = 1 i wtedy idziemy do biegu, jednak w animatorzze 
        //mamy zasade, �e gdy move jest mniejsze od 1 to przechodzi do Idle, jednak klikanj�c A warto�� move wynosi -1 wi�c po to jest wartosc 
        //bezwzgledna, zeby by�� to zawzse 1 niewa�nme czy klika si� A czy D a 0 zostaje dla nic nie klikni�tego
    }

    public void Jump(bool jumping)//metoda do aktywacji i dezaktywacji animacji skoku, przyjmuj�ca bool zmiennej do skoku
    {
        _anim.SetBool("Jumping", jumping);//Ustaweienie animacji, do skakania po jej nazwie taka co jest ustawiona w animatorze w Unity, oraz na podstawie zmiennej przyjmuj�cej
    }

    public void Attack()//Metoda do aktywacji animacji ataku
    {
        _anim.SetTrigger("Attack");//aktywacja triggera po nazwie
        _SwordAnim.SetTrigger("SwordAnim");//Aktywacja triggera
    }

    public void Death()
    {
        _anim.SetTrigger("Death");//funckaj do wywo�ania animacji �mierci gracza
    }

}
