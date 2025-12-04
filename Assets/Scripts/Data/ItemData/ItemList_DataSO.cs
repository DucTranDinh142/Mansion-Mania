using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item List", fileName = "List of items - ")]
public class ItemList_DataSO : ScriptableObject
{
    public Item_DataSO[] itemList;

    public Item_DataSO GetItemData(string saveID)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveID);
    }

#if UNITY_EDITOR
    // To avoid using UnityEditor in runtime
    [ContextMenu("***Auto-fill with all Item_DataSO***")]
    public void CollectItemsData()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Item_DataSO");

        itemList = guids
            .Select(guid => UnityEditor.AssetDatabase.LoadAssetAtPath<Item_DataSO>(
                UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
