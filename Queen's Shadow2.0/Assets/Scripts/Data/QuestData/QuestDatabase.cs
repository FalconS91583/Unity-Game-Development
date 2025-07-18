using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Quest Data/Quest Database", fileName = "QUEST DATABASE")]

public class QuestDatabase : ScriptableObject
{
    public QuestDataSO[] allQuests;

    public QuestDataSO GetQuestByID(string ID)
    {
        return allQuests.FirstOrDefault(q => q != null && q.questSaveID == ID);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all QuestItemSO")]
    public void CollectItemData()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");
        allQuests = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(quest => quest != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

    }
#endif
}
