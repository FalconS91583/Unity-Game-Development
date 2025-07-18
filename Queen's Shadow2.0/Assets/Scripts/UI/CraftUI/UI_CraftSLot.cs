using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSLot : MonoBehaviour
{
    private ItemDataSO itemToCraft;
    [SerializeField] private UI_CraftPreview craftPreview;

    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;


    public void SetupButton(ItemDataSO craftData)
    {
        this.itemToCraft = craftData;
        craftItemIcon.sprite = craftData.itemIcon;
        craftItemName.text  = craftData.itemName;
    }

    public void UpdateCraftPreview() => craftPreview.UpdateCraftPreview(itemToCraft);


}
