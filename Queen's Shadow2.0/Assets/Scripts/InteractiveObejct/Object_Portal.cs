using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour, ISaveable
{
    public static Object_Portal instance;

    public bool isActive {  get; private set; }

    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private string townSceneName = "Level 0";

    [SerializeField] private Transform repsawnPoint;
    [SerializeField] private bool canBeTriggerd;

    private string returnSceneName;
    private string currentSceneName;
    private bool returningFromTown;

    private void Awake()
    {
        instance = this;

        currentSceneName = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999); //Hiding teleport
    }

    public void ActivatePortal(Vector3 position, int facingDir = 1)
    {
        isActive = true;
        transform.position = position;
        SaveManager.instance.GetGameData().inScenePortals.Clear();

        if(facingDir == -1)
            transform.Rotate(0, 180,0);
    }

    public void DisableIfNeeded()
    {
        if(returningFromTown == false) return;

        SaveManager.instance.GetGameData().inScenePortals.Remove(currentSceneName);
        isActive = false;
        transform.position = new Vector3(9999, 9999);
    }

    private void UseTeleport()
    {
        string destinationScene = InTown() ? returnSceneName : townSceneName;

        GameManager.instance.ChangeScene(destinationScene, RespawnType.Portal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggerd == false) return;
        
        UseTeleport();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggerd = true;
    }

    public void SetTrigger(bool trigger) => canBeTriggerd = trigger;

    public Vector3 GetPosition() => repsawnPoint != null ? repsawnPoint.position : transform.position;

    private bool InTown() => currentSceneName == townSceneName;

    public void LoadData(GameData data)
    {
        if(InTown() && data.inScenePortals.Count > 0)
        {
            transform.position = defaultPosition;
            isActive = true;
        }
        else if(data.inScenePortals.TryGetValue(currentSceneName, out Vector3 portalPosition))
        {
            transform.position = portalPosition;
            isActive = true;
        }

        returningFromTown = data.returningFromTown;
        returnSceneName = data.portalDestinationInName;
    }

    public void SaveData(ref GameData data)
    {
        data.returningFromTown = InTown();

        if (isActive && InTown() == false)
        {
            data.inScenePortals[currentSceneName] = transform.position;
            data.portalDestinationInName = currentSceneName;
        }else
        {
            data.inScenePortals.Remove(currentSceneName);
        }

    }
}
