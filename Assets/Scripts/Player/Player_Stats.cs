using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffOf(string source)
    {
        return activeBuff.Contains(source) == false;
    }

    public void ApplyBuff(BuffEffectData[] buffToApply, float duration, string source)
    {
        StartCoroutine(BuffCoroutine(buffToApply, duration, source));
    }

    private IEnumerator BuffCoroutine(BuffEffectData[] buffToApply, float duration, string source)
    {
        activeBuff.Add(source);

        foreach (var buff in buffToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }
        yield return new WaitForSeconds(duration);

        foreach (var buff in buffToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}