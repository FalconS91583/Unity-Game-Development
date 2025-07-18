using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_EquipSlotParent uiEquipSlotParent;
    [SerializeField] private TextMeshProUGUI currencyText;
    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();

        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void OnEnable()
    {
        if (inventory == null)
            return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
        uiEquipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        currencyText.text = inventory.gold.ToString("N0") + "g.";
    }

    

}
