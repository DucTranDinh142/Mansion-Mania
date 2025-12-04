using UnityEngine;

public class KING_Spell : MonoBehaviour
{
    private Entity_Combat combat;

    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private Collider2D spellCollider2D;

    [SerializeField] private float damage = 50f;
    [SerializeField] ElementalEffectData effectData;

    private ElementType element;
    private bool hasDealtDamage = false;

    public void SetupSpell(Entity_Combat combat)
    {
        this.combat = combat;

        Destroy(gameObject, 2f);
    }

    private void EnableCollider() => spellCollider2D.enabled = true;
    private void DisableCollider() => spellCollider2D.enabled = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable == null) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float ran = UnityEngine.Random.Range(0, 3);
        switch (ran)
        {
            case 0: element = ElementType.None; break;
            case 1: element = ElementType.Ice; break;
            case 2: element = ElementType.Fire; break;
            case 3: element = ElementType.Lightning; break;
        }

        if (element != ElementType.None)
            collision.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(element, effectData);

        damagable.TakeDamage(damage, 0, element, null);

        hasDealtDamage = true;
        DisableCollider();
    }
    private void OnDestroy()
    {
        hasDealtDamage = false;
    }
}
