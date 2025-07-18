using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO dropData;

    [Header("Drop Descriptions")]
    [SerializeField] private int maxRarityAmout = 1200;
    [SerializeField] private int maxItemToDrop = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            
            DropItems();
        }
    }

    public virtual void DropItems()
    {
        if (dropData == null)
            return;

        List<ItemDataSO> itemToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemToDrop.Count, maxItemToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemToDrop[i]);
        }
    }

    public void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();
        float maxRarityAmount = this.maxRarityAmout;

        foreach (var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();

            if(Random.Range(0,100) <= dropChance)
                possibleDrops.Add(item);
        }

        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        foreach (var item in possibleDrops)
        {
            if(maxRarityAmout > item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmout = maxRarityAmout - item.itemRarity;
            }
        }
        return finalDrops;
    }
}
