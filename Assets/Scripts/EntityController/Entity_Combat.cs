using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    public float damage;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
    }
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            damagable.TakeDamage(damage, transform);
            entityVFX.CreateOnHitVFX(target.transform);
        }
    }
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheckTransform.position, targetCheckRadius, whatIsTarget);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheckTransform.position, targetCheckRadius);
    }
}
