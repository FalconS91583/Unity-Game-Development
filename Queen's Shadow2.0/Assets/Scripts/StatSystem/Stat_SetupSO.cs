using UnityEngine;

[CreateAssetMenu(menuName ="RPG Setup/Default Stat Setup", fileName ="Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damage")]

    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense - Physical Damge")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental Damge")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Major statts")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitalty;
}
