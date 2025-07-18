using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewMaterialSlot[] craftPreviewSlots;

    [Header("Item Preview Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewMaterialSlot>();

        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void ConfirmCraft()
    {
        if(itemToCraft == null)
        {
            buttonText.text = "Pick an item!";
            return; 
        }

        if (storage.CanCraftItem(itemToCraft))
        {
            storage.CraftItem(itemToCraft);
        }
        UpdateCraftPreviewSlot();

    }
    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();

        UpdateCraftPreviewSlot();
    }

    private void UpdateCraftPreviewSlot()
    {
        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemToCraft.itemData.craftRecipie.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipie[i];
            int aveliableAmout = storage.GetAveliableAmoutOf(requiredItem.itemData);
            int requiredAmout = requiredItem.stackSize;

            craftPreviewSlots[i].gameObject.SetActive(true);
            craftPreviewSlots[i].SetupMaterialSlot(requiredItem.itemData, aveliableAmout, requiredAmout);
        }
    }
}
