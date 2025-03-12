using UnityEngine;
using UnityEngine.SceneManagement;
//klasa odpowiadaj¹ca za dzia³anie menu
public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    //funkcja od za³adowania menu
    public void StartGame()
    {
        SceneManager.LoadScene("Snake");
    }
    //funkcja od wyjœæia z gry
    public void QuitGame()
    {
        Application.Quit();
    }
}
