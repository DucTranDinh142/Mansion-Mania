using System.Collections;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Entity_Stats statsToModify;

    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 3;
    [SerializeField] private bool canBeUsed = true;
    [Header("Floaty Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsed == false) return;

        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        sprite.color = Color.clear;
        
        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        ApplyBuff(false);

        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if(apply)
            statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
            statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);

        }
    }
}
