using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private int starttingHealth = 5;
    [SerializeField] private int currentHealth;
    int gameOverPriority = 20;
    [SerializeField] private Image[] shieldBar;

    [SerializeField] private CinemachineVirtualCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] private GameObject gameOverContainer;
    private void Awake()
    {
        currentHealth = starttingHealth;
        AdjustShieldUI();
    }

    public void TakeDamage(int amout)
    {
        currentHealth -= amout;
        AdjustShieldUI();

        if (currentHealth <= 0)
        {
            PlayerGameOver();
        }
    }

    private void PlayerGameOver()
    {
        weaponCamera.parent = null;
        deathVirtualCamera.Priority = gameOverPriority;
        gameOverContainer.SetActive(true);
        StarterAssetsInputs starterAssetInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetInputs.SetCursorState(false);
        Destroy(this.gameObject);
    }

    private void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBar.Length; i++)
        {
            if(i < currentHealth)
            {
                shieldBar[i].gameObject.SetActive(true);    
            }
            else
            {
                shieldBar[i].gameObject.SetActive(false);
            }
        }
    }
}
