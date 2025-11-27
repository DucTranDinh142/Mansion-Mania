using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;
    public event Action<Transform, float> OnHitTarget;

    private Entity_SFX entitySFX;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;

    public ScaleData basicAttackScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;
    
    private void Awake()
    {
        entitySFX = GetComponent<Entity_SFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();
    }
    public void PerformAttack()
    {
        bool targetGotHit = false;
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            ElementalEffectData effectData = new ElementalEffectData(entityStats, basicAttackScale);

            float elementalDamage = entityStats.GetElementalDamage(out ElementType element, basicAttackScale.elemental);
            float damage = entityStats.GetPhysicalDamage(out bool isCrit, basicAttackScale.physical);
            targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(element, effectData);

            if (targetGotHit)
            {
                OnDoingPhysicalDamage?.Invoke(damage);
                OnHitTarget?.Invoke(target.transform, damage);
                entityVFX.CreateOnHitVFX(target.transform, isCrit, element);
                entitySFX?.AttackHit();
            }
           
                
        }
        if(targetGotHit == false) entitySFX?.AttackMiss();
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
