using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform merchantInfo;
    [SerializeField] private Transform inventoryInfo;

    public void ShowToolTip
        (bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false, bool showMerchantInfo = false, bool showControlls = true)
    {
        base.ShowToolTip(show, targetRect);

        if(showControlls == true )
        {
            merchantInfo.gameObject.SetActive(showMerchantInfo);
            inventoryInfo.gameObject.SetActive(!showMerchantInfo);
        } else
        {
            merchantInfo.gameObject.SetActive(false);
            inventoryInfo.gameObject.SetActive(false);
        }

            int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = ($"Price:{price}x{itemToShow.stackSize} - {totalPrice}g.");
        string singleleStackPrice = ($"Price: {price}g.");

        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleleStackPrice;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();

        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color, itemToShow.itemData.itemName);

    }

    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 100) return "white"; // Common
        if (rarity <= 300) return "green"; // Uncommon
        if (rarity <= 600) return "blue";  // Rare
        if (rarity <= 850) return "purple";// Epic
        return "orange";                   // Legendary
    }

}
