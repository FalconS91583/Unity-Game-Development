using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _anim;//uchwyt w skrypcie do animatoira 
    private Animator _SwordAnim;//uchwyt dla animatora ale dla animacji machania miecza
    
    
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();//przypisanie do uchwytu componentu animatora, który jest  obiekterm dziecka Player, bo on jest w Sprite wieæ trzeba go wyszukac po dizecieym obieckie
        _SwordAnim = transform.GetChild(1).GetComponent<Animator>();//jest to przypisanie ichywtu ale w chierarchiczny sposób, poniewa¿ animacja miecza jest drugim elementem Gracza wiêc trzeba przejœc przez pierwszy obiekt i potem dostaæ siê do miecz 
        //dlatego inna formu³a przypisania uchywtu
    }

    public void Move(float move) //funckja do badania wartoœci float z player skryptu przyjmuj¹ca w³aœnie t¹ wartoœc stamt¹d
    {
        _anim.SetFloat("Move", Mathf.Abs(move));//Wywo³anie z animatora odpalenia Animacji o nazwie Move, na podstawie wartoœci zmiennej move
        //rozszerzonej o funckje matematyczna wartoœæ bezwzledna, bo klikanjaæ D wartoœæ move jest = 1 i wtedy idziemy do biegu, jednak w animatorzze 
        //mamy zasade, ¿e gdy move jest mniejsze od 1 to przechodzi do Idle, jednak klikanj¹c A wartoœæ move wynosi -1 wiêc po to jest wartosc 
        //bezwzgledna, zeby by³¹ to zawzse 1 niewa¿nme czy klika siê A czy D a 0 zostaje dla nic nie klikniêtego
    }

    public void Jump(bool jumping)//metoda do aktywacji i dezaktywacji animacji skoku, przyjmuj¹ca bool zmiennej do skoku
    {
        _anim.SetBool("Jumping", jumping);//Ustaweienie animacji, do skakania po jej nazwie taka co jest ustawiona w animatorze w Unity, oraz na podstawie zmiennej przyjmuj¹cej
    }

    public void Attack()//Metoda do aktywacji animacji ataku
    {
        _anim.SetTrigger("Attack");//aktywacja triggera po nazwie
        _SwordAnim.SetTrigger("SwordAnim");//Aktywacja triggera
    }

    public void Death()
    {
        _anim.SetTrigger("Death");//funckaj do wywo³ania animacji œmierci gracza
    }

}
