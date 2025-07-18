using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    private Vector3 lastDeathPosition;

    private string lastScenePlayed;
    private bool dataLoaded;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } 

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ContinuePlay()
    {
        ChangeScene(lastScenePlayed, RespawnType.None);
    }

    //public void SetLastDeathPosition(Vector3 position) => lastDeathPosition = position;
    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
    }

    public void ChangeScene(string sceneName, RespawnType repawnType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, repawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeOut();

        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        dataLoaded = false;
        yield return null;

        while(dataLoaded == false)
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn();
    
        Player player = Player.instance;

        if (player == null)
            yield break; 

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero) 
            player.TeleportPlayer(position);
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if(UI.instance != null)
        {
            return UI.instance.fadeScreenUI;
        }
        else
        {
            return FindFirstObjectByType<UI_FadeScreen>();
        }
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type == RespawnType.Portal)
        {
            Object_Portal portal = Object_Portal.instance;

            Vector3 position = portal.GetPosition(); 

            portal.SetTrigger(false);   
            portal.DisableIfNeeded();

            return position;
        }

        if(type == RespawnType.None)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointID(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                .Select(wp => wp.Getposition())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(waypoints).ToList();   //concat combine 2 list into 1

            if(selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions.OrderBy(position => Vector3.Distance(position, lastDeathPosition)).First();
        }

        return GetwaypointPosition(type);
    }

    private Vector3 GetwaypointPosition(RespawnType type)
    {
        var wawypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in wawypoints)
        {
            if(point.GetWaypointType() == type)
                return point.Getposition();
        }

        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lasrScenePlayed;
        lastDeathPosition = data.lastPlayerPosition;

        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Level 0";

        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
        {
            return;
        }

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lasrScenePlayed = currentScene;
        dataLoaded = false; 
    }
}
