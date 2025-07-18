using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "ShadowQueenData.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindISaveables();

        yield return null;

        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if(gameData == null)
        {
            Debug.Log("No save data, creating new");
            gameData = new GameData();
            return;
        }

        foreach (var saveables in allSaveables)
        {
            saveables.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var saveables in allSaveables)
        {
            saveables.SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData() => gameData;


    [ContextMenu("***** DELETE SAVE *****")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteData();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>().ToList();
    }

}
