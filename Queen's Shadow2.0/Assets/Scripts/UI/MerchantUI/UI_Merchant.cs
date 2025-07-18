using TMPro;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent equipSLots;
    [SerializeField] private TextMeshProUGUI currencyText;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player invenroty)
    {
        this.merchant = merchant;
        this.inventory = invenroty;

        this.inventory.OnInventoryChange += UpdateSlotUI;
        this.merchant.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchantSlots)
        {
            slot.SetupMerchantUI(merchant);
        }
    }

    private void UpdateSlotUI()
    {
        if (inventory == null)
            return;

        inventorySlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
        equipSLots.UpdateEquipmentSlots(inventory.equipList);

        currencyText.text = inventory.gold.ToString("N0") + "g.";
    }
}
