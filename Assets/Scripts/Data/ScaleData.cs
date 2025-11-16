using UnityEngine;
[System.Serializable]
public class ScaleData
{
    [Header("Damage")]
    public float physical = 1;
    public float elemental = 1;

    [Header("Chill Effect")]
    public float chillDuration = 3;
    public float chillSlowMultiplier = .2f;

    [Header("Burn Effect")]
    public float burnDuration = 5;
    public float burnDamageScale = .6f;
    public int burnTickPerSec = 2;

    [Header("Shock Effect")]
    public float shockDuration = 3;
    public float shockDamageScale = 1;
    public float shockCharge = .35f;
}