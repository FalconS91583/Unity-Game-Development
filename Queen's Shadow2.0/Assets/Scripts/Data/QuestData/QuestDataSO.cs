using UnityEditor;
using UnityEngine;
public enum RewardType { Merchant, Blacksmith, None };
public enum QuestType { Kill, Talk, Delivery}

[CreateAssetMenu(menuName = "RPG Setup/Quest Data/New Quest", fileName = "Quest - ")]
public class QuestDataSO : ScriptableObject
{
    public string questSaveID;
    [Space]
    public QuestType questType;
    public string questName;
    [TextArea] public string description;
    [TextArea] public string questGoal;

    public string questTargetID;
    public int requiredAmount;
    public ItemDataSO itemToDeliver;

    [Header("Reward")]
    public RewardType rewardType;
    public Inventory_Item[] rewardItems;


    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        questSaveID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

}
