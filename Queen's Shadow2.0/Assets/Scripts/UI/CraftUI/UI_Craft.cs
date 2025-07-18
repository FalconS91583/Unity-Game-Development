using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player inventory;

    private UI_CraftPreview craftPreviewUI;
    private UI_CraftSLot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;

    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.inventory;
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();

        craftPreviewUI = GetComponentInChildren<UI_CraftPreview>();
        craftPreviewUI.SetupCraftPreview(storage); 
        SetupCraftListButtons();
    }

    private void SetupCraftListButtons()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSLot>();

        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        craftListButtons = GetComponentsInChildren<UI_CraftListButton>();

        foreach (var button in craftListButtons)
        {
            button.SetCraftSlots(craftSlots);
        }
    }

    private void UpdateUI() => inventoryParent.UpdateSlots(inventory.itemList);

}
