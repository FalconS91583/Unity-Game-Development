using System;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Inventory_Item 
{
    private string itemId;

    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {  get; private set; }
    public ItemEffectDataSO itemEffect;

    public int buyPrice {  get; private set; }
    public float sellPrice { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * 0.35f;

        modifiers = EquipmentData()?.modifiers;

        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifer(mod.value, itemId);
        }
    }

    public void RemoveModifier(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifer(itemId);
        }
    }

    public void AddItemEffecy(Player player) => itemEffect?.Subscribe(player);
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipment)
            return equipment;

        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;


    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();

        if (itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for crafting");
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }

        if (itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDataDescription);
            sb.AppendLine("");
            sb.AppendLine("");

            return  sb.ToString();
        }


        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPrecentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();

            sb.AppendLine("+ " + modValue + " " + modType);
        }

        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique Effect");
            sb.AppendLine(itemEffect.effectDataDescription);
        }
        sb.AppendLine("");
        sb.AppendLine("");

        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CritChance: return "Critical Chance";
            case StatType.CritPower: return "Critical Power";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";

            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }

    private bool IsPrecentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}
