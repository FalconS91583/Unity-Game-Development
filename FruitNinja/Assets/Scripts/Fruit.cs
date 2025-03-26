using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;//referencja do obiektu pe�nego owocka
    public GameObject sliced;//referencja do obiektu przecietego owocka
    public AudioClip sliceSound; // Nowa zmienna przechowuj�ca d�wi�k przeci�cia owocka

    private Rigidbody fruitRigidbody;//zmienna przechowuj�ca referencj� do komponentu Rigidbody przypisanego do tego obiektu skryptu przechowuje fizyke i ruch obiekt�w
    private Collider fruitCollider;//zmienna przechowuj�ca referencj� do komponentu Collider przypisanego do tego obiektu skryptu przechowuje kolizje z innymi obiektami
    private ParticleSystem juiceParticleEffect;//zmienna przechowuj�ca referencj� do komponentu ParticleSystem (cz�steczkowego efektu) przypisanego do tego obiektu skryptu. ParticleSystem pozwala na odtwarzanie efekt�w cz�steczkowych, w tym wypadku efektu "soku" 
    private AudioSource audioSource; // Nowa zmienna przechowuj�ca komponent AudioSource
    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Ustawienie na 1.0, aby d�wi�k by� przestrzenny (3D)
        audioSource.volume = 1.0f; // Mo�esz zmieni� g�o�no�� d�wi�ku
        audioSource.loop = false;
        audioSource.clip = sliceSound;
    }//przypisywanie referencji do komponent�w

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        FindObjectOfType<GameMenager>().IncresaseScore();//Znajdujemy i pobieramy referencj� do komponentu GameManager i wywo�ujemy funckje IncresaseScore

        whole.SetActive(false);
        sliced.SetActive(true);
        //wy��czamy obiekt ca�y i aktywujemy przeciety 
        fruitCollider.enabled = false;//wy��czamy collider aby przeci�te cz�sci nie kolidowa�y ze sob�
        juiceParticleEffect.Play();//odtwarzamy efekt cz�steczkowy do animacji soku po przeci�ciu 


        float angle = Mathf.Atan2(direction.y, direction.y) * Mathf.Rad2Deg; 
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //obliczamy k�t kierunku ci�cia i ustawiamy rotacje
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();//tablica na wszytkie ,,dzieci" komponentu znalezione w rigidbody jako przeciete czesci owocka

        foreach (Rigidbody slice in slices)//Przechodzimy przez wszystkie dzieci obiektu sliced, kt�re maj� komponent Rigidbody (np. cz�ci owoca).
        {
            slice.velocity = fruitRigidbody.velocity;//ustawiamy predko�� przecietego owocka na predko�� oryginalnego owoca
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);//do przecietego owocu dodajmy impuls poprzez mnozenie kierunku i si�y aby symulowac efekt odrzutu
        }
    if (sliceSound != null && !audioSource.isPlaying)
    {
        audioSource.PlayOneShot(sliceSound); // Odtwarzamy d�wi�k przeci�cia
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }//jezeli otagowany noz jako player bedzie w kolizji z owockiem pobieramy skrypt Blade i wywo�ujemy metode slice z przekazaniem kierunku ci�cia, pozycji myszki oraz si�y ci�cia
    }
}
