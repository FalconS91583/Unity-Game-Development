using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Equipment Item", fileName = "Equipment Data - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] modifiers;
}

[System.Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}