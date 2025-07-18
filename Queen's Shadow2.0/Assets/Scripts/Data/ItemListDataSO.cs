using UnityEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item List ", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

    public ItemDataSO GetItemData(string saveID)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveID);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
        itemList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray(); 

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        
    }
#endif
}
