using UnityEngine;
using UnityEngine.SceneManagement;
//klasa odpowiadaj�ca za dzia�anie menu
public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    //funkcja od za�adowania menu
    public void StartGame()
    {
        SceneManager.LoadScene("Snake");
    }
    //funkcja od wyj��ia z gry
    public void QuitGame()
    {
        Application.Quit();
    }
}
