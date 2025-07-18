using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviewMaterialSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    public void SetupMaterialSlot(ItemDataSO itemData, int aveliableAmout, int requiredAmout)
    {
        materialIcon.sprite = itemData.itemIcon;
        materialNameValue.text = itemData.itemName + " - " + aveliableAmout + " / " + requiredAmout;
    }
}
