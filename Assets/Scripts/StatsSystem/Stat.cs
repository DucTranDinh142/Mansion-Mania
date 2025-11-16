using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool wasModified = true;
    private float finalValue;

    public float GetValue()
    {
        if (wasModified)
        {
            finalValue = GetFinalValue();
            wasModified = false;
        }

        return finalValue;
    }
    public void AddModifier(float value, string source)
    {
        StatModifier modifierToAdd = new StatModifier(value, source);
        modifiers.Add(modifierToAdd);
        wasModified = true;
    }
    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        wasModified = true;
    }
    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }

        return finalValue;
    }
    public void SetBaseValue(float value) => baseValue = value;
}

[System.Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}