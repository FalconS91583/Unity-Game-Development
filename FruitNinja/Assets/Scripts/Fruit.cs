using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;//referencja do obiektu pe³nego owocka
    public GameObject sliced;//referencja do obiektu przecietego owocka
    public AudioClip sliceSound; // Nowa zmienna przechowuj¹ca dŸwiêk przeciêcia owocka

    private Rigidbody fruitRigidbody;//zmienna przechowuj¹ca referencjê do komponentu Rigidbody przypisanego do tego obiektu skryptu przechowuje fizyke i ruch obiektów
    private Collider fruitCollider;//zmienna przechowuj¹ca referencjê do komponentu Collider przypisanego do tego obiektu skryptu przechowuje kolizje z innymi obiektami
    private ParticleSystem juiceParticleEffect;//zmienna przechowuj¹ca referencjê do komponentu ParticleSystem (cz¹steczkowego efektu) przypisanego do tego obiektu skryptu. ParticleSystem pozwala na odtwarzanie efektów cz¹steczkowych, w tym wypadku efektu "soku" 
    private AudioSource audioSource; // Nowa zmienna przechowuj¹ca komponent AudioSource
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
        audioSource.spatialBlend = 1.0f; // Ustawienie na 1.0, aby dŸwiêk by³ przestrzenny (3D)
        audioSource.volume = 1.0f; // Mo¿esz zmieniæ g³oœnoœæ dŸwiêku
        audioSource.loop = false;
        audioSource.clip = sliceSound;
    }//przypisywanie referencji do komponentów

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        FindObjectOfType<GameMenager>().IncresaseScore();//Znajdujemy i pobieramy referencjê do komponentu GameManager i wywo³ujemy funckje IncresaseScore

        whole.SetActive(false);
        sliced.SetActive(true);
        //wy³¹czamy obiekt ca³y i aktywujemy przeciety 
        fruitCollider.enabled = false;//wy³¹czamy collider aby przeciête czêsci nie kolidowa³y ze sob¹
        juiceParticleEffect.Play();//odtwarzamy efekt cz¹steczkowy do animacji soku po przeciêciu 


        float angle = Mathf.Atan2(direction.y, direction.y) * Mathf.Rad2Deg; 
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //obliczamy k¹t kierunku ciêcia i ustawiamy rotacje
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();//tablica na wszytkie ,,dzieci" komponentu znalezione w rigidbody jako przeciete czesci owocka

        foreach (Rigidbody slice in slices)//Przechodzimy przez wszystkie dzieci obiektu sliced, które maj¹ komponent Rigidbody (np. czêœci owoca).
        {
            slice.velocity = fruitRigidbody.velocity;//ustawiamy predkoœæ przecietego owocka na predkoœæ oryginalnego owoca
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);//do przecietego owocu dodajmy impuls poprzez mnozenie kierunku i si³y aby symulowac efekt odrzutu
        }
    if (sliceSound != null && !audioSource.isPlaying)
    {
        audioSource.PlayOneShot(sliceSound); // Odtwarzamy dŸwiêk przeciêcia
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }//jezeli otagowany noz jako player bedzie w kolizji z owockiem pobieramy skrypt Blade i wywo³ujemy metode slice z przekazaniem kierunku ciêcia, pozycji myszki oraz si³y ciêcia
    }
}
