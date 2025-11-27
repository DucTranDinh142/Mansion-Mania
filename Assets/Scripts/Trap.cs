using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float velocityScale = .5f;

    private bool hasDealtDamage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable == null) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float fallSpeed = Mathf.Abs(rb.linearVelocity.y);

        float finalDamage = damage + (fallSpeed * velocityScale);

        damagable.TakeDamage(finalDamage, 0, ElementType.None, null);

        hasDealtDamage = true;
    }
private void OnTriggerExit2D(Collider2D collision)
    {
        hasDealtDamage = false;
    }
}
