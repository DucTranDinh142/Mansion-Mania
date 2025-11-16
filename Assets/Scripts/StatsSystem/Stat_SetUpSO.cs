using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetUpSO : ScriptableObject
{
    [Header("Major Stats")]
    public float vitality;
    public float strength;
    public float agility;
    public float intelligence;
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance = 1;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - ElementalEffectData Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense - Physical Damage")]
    public float armor;
    public float evasion;

    [Header("Defense - ElementalEffectData Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;
}
