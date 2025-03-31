using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable//dziedziczymy dodatkowo po interfejsie od damage
{
    private Rigidbody2D _rigid;//uchwyt dla komponentu rigidbody
    [SerializeField]//Podowduje to, ¿e dan¹ zmienna jest praywatna dla wszystkich lecz mo¿emy j¹ edytow¹c w edytorze
    private float _jumpForce = 5.0f;//zmienna do przechowywywania skoku
    [SerializeField]
    private bool _Grounded = false;//zmienna sprawdzaj¹ca czy jestœy na ziemii
    [SerializeField]
    private LayerMask _groundLayer;//zmienna do przechowywania warstwy tego po czym chodzi gracz
    private bool resetJump = false; //zmienna do powstrezymywania gracza od nadmiernego skakania
    [SerializeField] 
    private float _speed = 2.5f;

    private PlayerAnimations _Anim;//uchwyt do skryptu z animacjami
    private SpriteRenderer _Sprite;//uchwyt dla componentu na obiekcie Sprite w unity
    private SpriteRenderer _SwordArcSprite;//uchwyt dla komponentu sprite rendereer w unity

    public int diamonds;//zmienna pod trzymanie iloœci diamentó gracza
    public int health { get; set; }//zmienna do wype³nienia zasad interfejsu
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();//skrypt jest na graczu, który ma  rigidbody wiêc nie trzbea go szukaæ po prostu siê go przypisuje
        _Anim = GetComponent<PlayerAnimations>();//przypisanie do zmiennej skryptu na poc¿atku gry(Teraz to jest na ym sammym obieckie wiêc wysterczy zwyk³e GetComponent
        _Sprite = GetComponentInChildren<SpriteRenderer>();//przypisanie do zmiennej wymaganego komponentu obiektu w Unity
        _SwordArcSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();//jest to przypisanie ichywtu ale w chierarchiczny sposób, poniewa¿ animacja miecza jest drugim elementem Gracza wiêc trzeba przejœc przez pierwszy obiekt i potem dostaæ siê do miecz 
        //dlatego inna formu³a przypisania uchywtu
        health = 4;
    }

    void Update()
    {
        //wywo³anie metod
        Movement();

        if(Input.GetMouseButtonDown(0) && IsGrounded() == true)//je¿eli jest klikniety lewy przzycisk myszy(bazowo oznqacza siê jako 0) oraz jestesmy na ziemii
        {
            _Anim.Attack();//uruchumienie metody z playeranimations do aktywacji animacji ataku
        }

    }

    void Movement()//funckja do poruszania siê gracza i skoku
    {
        float move = Input.GetAxisRaw("Horizontal");//zmienna, który przyjmuje tylko poruszanie horyzontale czyli, lewo prawo, przy kliknieciu w,s lub strza³ek góra dó³ zwraca 0
                                                    //getAxis powoduje, ¿e tan float przyjmuje wartoœci po kolei czyli trzymamy D to jest 0.1 , 0.2 itd, przez co postaæ siê œlizga przy zatrzymywaniu, gdy damy raw to wartoœci odrazu ustawiaj¹ siê na 0  1
        _Grounded = IsGrounded();

        Flip(move);//Wywo³anie metody do obracania postaci¹
        /* stare podejœcie
        if (Input.GetKeyDown(KeyCode.Space) && _Grounded == true)//je¿eli klikeiety jest przycisk spacji i jesteœmy na ziemii 
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);//to modyfikujemy nasz¹ pozycje w grze nma x na jakim jesteœmy i ustawion¹ wartoœcia na y
            _Grounded = false;// i ustawiamy uziemienie na false
            resetJump = true;//ustawienie przerwy na skakanie
            StartCoroutine(ResetJumpRoutine());//uruchomienie opóŸnienia
        }
        */
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _Anim.Jump(true);//Wywo³anie metody Jump z skryptu PlayerAnimations
        
        }
        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);//zaczêcie modyfikacji velocity kompomentu z rigidbody  i przypisaywanie do niego nowych zmiennych, gdzie pierwsze pobiera nasz¹ zmienn¹, grugie bierze sobie jakiekoleik tam y jest akutalne, nie dajemy 0, bo wtedy ci¹gle by siê resetowa³o, a np jak gracz spada to ma spaœæ a nie reset na 0


        _Anim.Move(move);//wziêcie metody Move z skryptu PlayerAnimation za pomoc¹ uchwytu i przekazanie mu wartoœci zmiennej move z tego skryptu
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

    void Flip(float move)//Funckaj przyjmuj¹ca wartoœc zmiennej move, funcka odpowiada za obrót postaci w strone biegu
    {
        //logika do obracania postaci¹ bazuj¹c od tego w która strone biegnie, równie¿ ligika do obrcania animacji miecza bazuj¹c¹ na tym samym
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

    IEnumerator ResetJumpRoutine()//korutyna do pozwalania nam robiæ coœ po jakimœ up³ywanie czasu 
    {
        resetJump = true;
        yield return new WaitForSeconds(1f);//zwraca nam aby przed zrobieniem czegoœ poczeka³o 0.1 sekundy
        resetJump = false;//i ustawiamy reset jump na false
    }

    public void Damage()//funckcja do spe³nienia zasad interfejsu 
    {
        if(health < 1) //if do zapobiegania ci¹g³ego wywo³ywania siê animacji umierania
        {
            return;
        }
        health--;//dekremantacja zdrowia
        UIManager.Instance.UpdateLives(health);//przekazanie do UIManagera do funcke od zarz¹dania ¿yciami aktualn¹ wartoœæ zdrowia gracza
        if (health < 1)//je¿eli ¿yæ jest mniej ni¿ 1
        {
            _Anim.Death();//wywo³ujemy metode która odpala animacje deda
        }
    }

    public void AddGems(int Amout)//funkcja, która bêdzie wywo³ywana do operacji zbierania diamentów
    {
        diamonds += Amout;//diamenciki wynosz¹ tyle ile ich mamy 
        UIManager.Instance.UpdateGameCount(diamonds);//odwo³anie siê do funckji do aktualizacji stanu diamencików z UIManagera przekazuj¹c mu przypisana liczbe diamntów
       
    }

    /* 
     * stare podejscie
     * 


      stare podejœcie
         void IsGrounded()//funkcja fo sprawdzania gracza na pod³o¿u
         {
             RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer.value);//raycast to inforamcja zwrócona o obiekcie wykrytym przez raycasta
             Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.green);//dziêki tej linii widziemy gdzie uderza nasz Ray gracza

             if (hitInfo.collider != null)// jezeli raycast wykreyje, ¿e w coœ walneliœmy
             {
                 if (resetJump == false)
                 {
                     _Grounded = true;//no to grounded jest true
                 }

             }
         }
     */


}
