using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _Lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleshotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject Right_Engine, Left_Engine;


    [SerializeField]
    private int _score;
    [SerializeField]
private int _highScore;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoudClip;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()    
    {
        transform.position = new Vector3(0, 0, 00);
        //Dostanie sie do skryptu z poziomu  gracza do spawn managera 
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>(); 
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }
        if(_uiManager == null)
        {
            Debug.Log("UIManager is null");
        } 
        if( _audioSource == null)
        {
            Debug.Log("Audio Source is null");
        }
        else
        {
            _audioSource.clip = _laserSoudClip;
        }
    }

    // Update is called once per frame
    void Update()
    {       
        CalculateMovement();
        
        if (Input.GetKeyUp(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
       
            transform.Translate(direction * _speed * Time.deltaTime);       

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //optymalizacyjne wyjscie 
        //transform.Translate(new Vector3(horizontalinput, verticalInput, 0) * _speed * Time.deltaTime);
        //lub
        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //transform>translate(direction * _speed * Time.feltaTime);
        //Optymalizacja ifa dla y 
        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        /*bezz przechodzenia ścian
         * if (transform.position.x >=9f)
        {
            transform.position = new Vector3(9f, transform.position.y, 0);
        } else if(transform.position.x <= -9f)
        {
            transform.position = new Vector3(-9f, transform.position.y, 0);
        }
        */
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser() 
    {

     _canFire = Time.time + _fireRate;   
    if(_isTripleshotActive == true)
        {
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

    _audioSource.Play();
    }
    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _Lives--;

        if(_Lives == 2)
        {
            Left_Engine.SetActive(true);
        }
        else if(_Lives == 1)
        {
            Right_Engine.SetActive(true);
        }


        _uiManager.updateLives(_Lives);
        if(_Lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleshotActive = true;
        StartCoroutine(TriplShotPowerDownRoutine());
    }
    IEnumerator TriplShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleshotActive = false;
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true); 
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
}
