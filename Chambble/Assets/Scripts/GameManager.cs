using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject GameoOverScreen;

    private void Awake()
    {
        GameoOverScreen.SetActive(false);
    }
    public bool isDead;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    public void StartGame()
    {
        isDead = false;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Death()
    {
        if (isDead)
        {
            Time.timeScale = 0;
            GameoOverScreen.SetActive(true);
        }
    }
}
