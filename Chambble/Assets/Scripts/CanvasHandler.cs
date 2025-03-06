using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasHandler : MonoBehaviour
{
    public void LoadSceneGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
}
