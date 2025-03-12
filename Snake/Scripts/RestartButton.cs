using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//klasa od resetowania gry
public class RestartButton : MonoBehaviour
{
    private Button restartButton;

    private void Start()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
