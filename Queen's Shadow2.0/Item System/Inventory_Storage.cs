using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player inventory { get; private set; }
    public List<Inventory_Item> materialStash;

    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeMaterials(itemToCraft);
        inventory.AddItem(itemToCraft);
    }

    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && inventory.CanAddItem(itemToCraft);
    }
    private void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipie)
        {
            int amountToConsume = requiredItem.stackSize;

            amountToConsume = amountToConsume - ConsumedMaterialsAmount(inventory.itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialsAmount(itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialsAmount(materialStash, requiredItem);

        }
    }

    private int ConsumedMaterialsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;

        foreach (var item in itemList)
        {
            if (item.itemData != neededItem.itemData)
                continue;

            int removeAmount = Math.Min(item.stackSize, amountNeeded - consumedAmount);
            item.stackSize = item.stackSize - removeAmount;
            consumedAmount = consumedAmount + removeAmount;

            if (item.stackSize <= 0)
                itemList.Remove(item);

            if (consumedAmount >= amountNeeded)
                break;

        }

        return consumedAmount;

    }

    public bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterials in itemToCraft.itemData.craftRecipie)
        {
            if (GetAveliableAmoutOf(requiredMaterials.itemData) < requiredMaterials.stackSize)
                return false;
        }
        return true;
    }

    public int GetAveliableAmoutOf(ItemDataSO rquiredItem)
    {
        int amout = 0;

        foreach (var item in inventory.itemList)
        {
            if (item.itemData == rquiredItem)
                amout = amout + item.stackSize;
        }

        foreach (var item in itemList)
        {
            if (item.itemData == rquiredItem)
                amout = amout + item.stackSize;
        }

        foreach (var item in materialStash)
        {
            if (item.itemData == rquiredItem)
                amout = amout + item.stackSize;
        }

        return amout;
    }

    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(newItemToAdd);
        }

        TriggerUpdateUI();
        materialStash = materialStash.OrderBy(item => item.itemData.name).ToList();
    }

    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        return materialStash.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());
    }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmout = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmout; i++)
        {
            // Storage Check
            if (CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);
                inventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmout = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmout; i++)
        {

            // Player Inv Check
            if (inventory.CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);

                RemoveOneItem(item);
                inventory.AddItem(itemToAdd);
            }

        }

        TriggerUpdateUI();
    }

    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);


        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;


                if (data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;

                data.storageItems[saveId] += item.stackSize;
            }
        }

        data.storageMaterials.Clear();


        foreach (var item in materialStash)
        {
            if (item != null && item.itemData != null)
            {
                string saveID = item.itemData.saveID;

                if (data.storageMaterials.ContainsKey(saveID) == false)
                    data.storageMaterials[saveID] = 0;

                data.storageMaterials[saveID] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }


        foreach (var item in data.storageMaterials)
        {
            string saveID = item.Key;
            int stackSize = item.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveID);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found" + saveID);
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddMaterialToStash(itemToLoad);
            }
        }
    }
}
