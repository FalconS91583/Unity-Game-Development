using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesLeftText;
    [SerializeField] private GameObject winText;

    private int enemiesLeft = 0;

    const string ENEMIES_LEFT_STRING = "Enemies Left: ";
    public void AdjustEnemies(int amout)
    {
        enemiesLeft += amout;
        enemiesLeftText.text = ENEMIES_LEFT_STRING + enemiesLeft.ToString();

        if(enemiesLeft <= 0)
        {
            winText.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
