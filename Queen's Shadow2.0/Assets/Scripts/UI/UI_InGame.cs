using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skillsSlots;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlkider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Quick Item Slot")]
    [SerializeField] private float yOffsetQuickItemParent = 150;
    [SerializeField] private Transform QuickItemOptionsParent;
    private UI_QuickItemSlotOption[] quickItemOptions;
    private UI_QuickItemSlot[] quickItemSlots;

    private void Start()
    {
        quickItemSlots = GetComponentsInChildren<UI_QuickItemSlot>();

        player = FindFirstObjectByType<Player>();
        player.health.OnHealthUpdate += UpdateHealthBar;

        inventory = player.inventoryPlayer;

        inventory.OnInventoryChange += UpdateQuickSlotsUI;
        inventory.OnQuickSlotUsed += PlayQuickSlotFeedback;
    }

    public void PlayQuickSlotFeedback(int slotNumber) => quickItemSlots[slotNumber].SimulateButtonFeedback();

    public void UpdateQuickSlotsUI()
    {
        Inventory_Item[] quickItems = inventory.quickItems;

        for (int i = 0; i < quickItems.Length; i++)
        {
            quickItemSlots[i].UpdateQuickSLotUI(quickItems[i]);
        }  
    }

    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot, RectTransform targetRect)
    {
        if(quickItemOptions == null)
            quickItemOptions = QuickItemOptionsParent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);

        List<Inventory_Item> consumableItems =
            inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Consumable);

        for (int i = 0; i < quickItemOptions.Length; i++)
        {
            if (i < consumableItems.Count)
            {
                quickItemOptions[i].gameObject.SetActive(true);
                quickItemOptions[i].SetupOption(quickItemSlot, consumableItems[i]);
            }
            else
                quickItemOptions[i].gameObject.SetActive(false);
        }

        QuickItemOptionsParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
    }

    public void HideQuickItemOptions() => QuickItemOptionsParent.position = new Vector3(0, 9999);

    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        if(skillsSlots == null)
            skillsSlots = GetComponentsInChildren<UI_SkillSlot>(true);

        foreach (var slot in skillsSlots)
        {
            if(slot.skillType == skillType)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }

        return null;
    }

    private void UpdateHealthBar()
    {
        float currentHealth = Mathf.RoundToInt(player.health.GetCurrentHealth());
        float maxHealth = player.stats.GetMaxHealth();

        float sizeDiffrence = Mathf.Abs(maxHealth - healthRect.sizeDelta.x);

        if (sizeDiffrence > 0.1f)
            healthRect.sizeDelta = new Vector2(maxHealth, healthRect.sizeDelta.y);

        healthText.text = currentHealth + "/" + maxHealth;
        healthSlkider.value = player.health.GetHealthProcentage();
    }
}
