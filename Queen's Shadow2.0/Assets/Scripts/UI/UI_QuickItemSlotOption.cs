using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;
    private UI_InGame inGameUI;

    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot, Inventory_Item itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSLotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }
}
