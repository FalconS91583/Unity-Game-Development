using UnityEngine;
using UnityEngine.SceneManagement;

public class ColisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip crash;
    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    private AudioSource audioSource;

    private bool isControlable = true;
    private bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKey();
    }

    private void RespondToDebugKey()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollidable = !isCollidable;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(!isControlable || !isCollidable)
            return;

        switch (other.gameObject.tag)
        {
            case "Frienldy":
                Debug.Log("Frielndy pallet");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        isControlable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadMainScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        isControlable = false;
        audioSource.Stop();
        successParticles.Play();
        audioSource.PlayOneShot(success);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void LoadMainScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

}
