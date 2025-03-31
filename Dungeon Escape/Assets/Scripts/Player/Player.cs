using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable//dziedziczymy dodatkowo po interfejsie od damage
{
    private Rigidbody2D _rigid;//uchwyt dla komponentu rigidbody
    [SerializeField]//Podowduje to, �e dan� zmienna jest praywatna dla wszystkich lecz mo�emy j� edytow�c w edytorze
    private float _jumpForce = 5.0f;//zmienna do przechowywywania skoku
    [SerializeField]
    private bool _Grounded = false;//zmienna sprawdzaj�ca czy jest�y na ziemii
    [SerializeField]
    private LayerMask _groundLayer;//zmienna do przechowywania warstwy tego po czym chodzi gracz
    private bool resetJump = false; //zmienna do powstrezymywania gracza od nadmiernego skakania
    [SerializeField] 
    private float _speed = 2.5f;

    private PlayerAnimations _Anim;//uchwyt do skryptu z animacjami
    private SpriteRenderer _Sprite;//uchwyt dla componentu na obiekcie Sprite w unity
    private SpriteRenderer _SwordArcSprite;//uchwyt dla komponentu sprite rendereer w unity

    public int diamonds;//zmienna pod trzymanie ilo�ci diament� gracza
    public int health { get; set; }//zmienna do wype�nienia zasad interfejsu
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();//skrypt jest na graczu, kt�ry ma  rigidbody wi�c nie trzbea go szuka� po prostu si� go przypisuje
        _Anim = GetComponent<PlayerAnimations>();//przypisanie do zmiennej skryptu na poc�atku gry(Teraz to jest na ym sammym obieckie wi�c wysterczy zwyk�e GetComponent
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie do zmiennej wymaganego komponentu obiektu w Unity
        _SwordArcSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();//jest to przypisanie ichywtu ale w chierarchiczny spos�b, poniewa� animacja miecza jest drugim elementem Gracza wi�c trzeba przej�c przez pierwszy obiekt i potem dosta� si� do miecz 
        //dlatego inna formu�a przypisania uchywtu
        health = 4;
    }

    void Update()
    {
        //wywo�anie metod
        Movement();

        if(Input.GetMouseButtonDown(0) && IsGrounded() == true)//je�eli jest klikniety lewy przzycisk myszy(bazowo oznqacza si� jako 0) oraz jestesmy na ziemii
        {
            _Anim.Attack();//uruchumienie metody z playeranimations do aktywacji animacji ataku
        }

    }

    void Movement()//funckja do poruszania si� gracza i skoku
    {
        float move = Input.GetAxisRaw("Horizontal");//zmienna, kt�ry przyjmuje tylko poruszanie horyzontale czyli, lewo prawo, przy kliknieciu w,s lub strza�ek g�ra d� zwraca 0
                                                    //getAxis powoduje, �e tan float przyjmuje warto�ci po kolei czyli trzymamy D to jest 0.1 , 0.2 itd, przez co posta� si� �lizga przy zatrzymywaniu, gdy damy raw to warto�ci odrazu ustawiaj� si� na 0  1
        _Grounded = IsGrounded();

        Flip(move);//Wywo�anie metody do obracania postaci�
        /* stare podej�cie
        if (Input.GetKeyDown(KeyCode.Space) && _Grounded == true)//je�eli klikeiety jest przycisk spacji i jeste�my na ziemii 
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);//to modyfikujemy nasz� pozycje w grze nma x na jakim jeste�my i ustawion� warto�cia na y
            _Grounded = false;// i ustawiamy uziemienie na false
            resetJump = true;//ustawienie przerwy na skakanie
            StartCoroutine(ResetJumpRoutine());//uruchomienie op�nienia
        }
        */
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _Anim.Jump(true);//Wywo�anie metody Jump z skryptu PlayerAnimations
        
        }
        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);//zacz�cie modyfikacji velocity kompomentu z rigidbody  i przypisaywanie do niego nowych zmiennych, gdzie pierwsze pobiera nasz� zmienn�, grugie bierze sobie jakiekoleik tam y jest akutalne, nie dajemy 0, bo wtedy ci�gle by si� resetowa�o, a np jak gracz spada to ma spa�� a nie reset na 0


        _Anim.Move(move);//wzi�cie metody Move z skryptu PlayerAnimation za pomoc� uchwytu i przekazanie mu warto�ci zmiennej move z tego skryptu
    }

    bool IsGrounded()//Metoda do sprawdzania czy gracz jest na ziemii czy nie
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 8);
        Debug.DrawRay(transform.position, Vector2.down  * 0.6f, Color.green);
        if (hitInfo.collider != null)
        {
            if(resetJump == false)  
            {
                _Anim.Jump(false);
                return true;
            }
            
        }
        return false;
    }

    void Flip(float move)//Funckaj przyjmuj�ca warto�c zmiennej move, funcka odpowiada za obr�t postaci w strone biegu
    {
        //logika do obracania postaci� bazuj�c od tego w kt�ra strone biegnie, r�wnie� ligika do obrcania animacji miecza bazuj�c� na tym samym
        if (move < 0)
        {
            _Sprite.flipX = true;
            //warunki do obracamia efektu miecza
            _SwordArcSprite.flipX = true;
            _SwordArcSprite.flipY = true;

            Vector3 newPose = _SwordArcSprite.transform.localPosition;
            newPose.x = -0.2f;
            _SwordArcSprite.transform.localPosition = newPose;
        }
        else if (move > 0)
        {
            _Sprite.flipX = false;
            //warunki do obracamia efektu miecza
            _SwordArcSprite.flipX = false;
            _SwordArcSprite.flipY = false;

            Vector3 newPose = _SwordArcSprite.transform.localPosition;
            newPose.x = 0.2f;
            _SwordArcSprite.transform.localPosition = newPose;
        }
    }

    IEnumerator ResetJumpRoutine()//korutyna do pozwalania nam robi� co� po jakim� up�ywanie czasu 
    {
        resetJump = true;
        yield return new WaitForSeconds(1f);//zwraca nam aby przed zrobieniem czego� poczeka�o 0.1 sekundy
        resetJump = false;//i ustawiamy reset jump na false
    }

    public void Damage()//funckcja do spe�nienia zasad interfejsu 
    {
        if(health < 1) //if do zapobiegania ci�g�ego wywo�ywania si� animacji umierania
        {
            return;
        }
        health--;//dekremantacja zdrowia
        UIManager.Instance.UpdateLives(health);//przekazanie do UIManagera do funcke od zarz�dania �yciami aktualn� warto�� zdrowia gracza
        if (health < 1)//je�eli �y� jest mniej ni� 1
        {
            _Anim.Death();//wywo�ujemy metode kt�ra odpala animacje deda
        }
    }

    public void AddGems(int Amout)//funkcja, kt�ra b�dzie wywo�ywana do operacji zbierania diament�w
    {
        diamonds += Amout;//diamenciki wynosz� tyle ile ich mamy 
        UIManager.Instance.UpdateGameCount(diamonds);//odwo�anie si� do funckji do aktualizacji stanu diamencik�w z UIManagera przekazuj�c mu przypisana liczbe diamnt�w
       
    }

    /* 
     * stare podejscie
     * 


      stare podej�cie
         void IsGrounded()//funkcja fo sprawdzania gracza na pod�o�u
         {
             RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer.value);//raycast to inforamcja zwr�cona o obiekcie wykrytym przez raycasta
             Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.green);//dzi�ki tej linii widziemy gdzie uderza nasz Ray gracza

             if (hitInfo.collider != null)// jezeli raycast wykreyje, �e w co� walneli�my
             {
                 if (resetJump == false)
                 {
                     _Grounded = true;//no to grounded jest true
                 }

             }
         }
     */


}
