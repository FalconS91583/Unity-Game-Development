using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;


    public int gold = 10000;

    public List<Inventory_EquipmentSlot> equipList;
    public Inventory_Storage storage { get; private set; }

    [Header("Quick Item Slot")]
    public Inventory_Item[] quickItems = new Inventory_Item[2];



    protected override void Awake()
    {
        base.Awake();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        TriggerUpdateUI();
    }

    public void TryuseQuickItem(int passedSlotNumber)
    {
        int slotnumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotnumber];

        if (itemToUse == null)
            return;

        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotnumber] = FindSameItem(itemToUse);
        }

        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotnumber);
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                Equipitem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null);
        Equipitem(inventoryItem, slotToReplace);
    }

    private void Equipitem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPrecent = player.health.GetHealthProcentage();

        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddItemEffecy(player);

        player.health.SetHealthToPrecent(savedHealthPrecent);

        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
            return;

        float savedhealthPrecent = player.health.GetHealthProcentage();

        var slotToUnEquip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnEquip != null)
            slotToUnEquip.equipedItem = null;

        itemToUnequip.RemoveModifier(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHealthToPrecent(savedhealthPrecent);
        AddItem(itemToUnequip);
    }

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveID = item.itemData.saveID;

                if (data.inventory.ContainsKey(saveID) == false)
                    data.inventory[saveID] = 0;

                data.inventory[saveID] += item.stackSize;
            }
        }

        foreach (var slot in equipList)
        {
            if (slot.HasItem())
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;

        foreach (var item in data.inventory)
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
                AddItem(itemToLoad);
            }
        }

        foreach (var entry in data.equipedItems)
        {
            string saveID = entry.Key;
            ItemType loadedSlotType = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveID);
            Inventory_Item itemToLoad = new Inventory_Item(itemData);

            var slot = equipList.Find(slot => slot.slotType == loadedSlotType && slot.HasItem() == false);

            slot.equipedItem = itemToLoad;
            slot.equipedItem.AddModifiers(player.stats);
            slot.equipedItem.AddItemEffecy(player);
        }

        TriggerUpdateUI();
    }
}
