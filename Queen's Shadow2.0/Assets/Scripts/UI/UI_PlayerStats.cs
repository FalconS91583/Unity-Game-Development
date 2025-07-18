using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlot;
    private Inventory_Player inventory;

    private void Awake()
    {
        uiStatSlot = GetComponentsInChildren<UI_StatSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var statSlot in uiStatSlot)
        {
            statSlot.UpdateStatValue();
        }
    }
}
