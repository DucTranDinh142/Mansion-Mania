using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected GameObject VFXPerfab;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheckTransform;
    [SerializeField] protected float checkRadius = 1;


    protected Rigidbody2D skillObjectRigidbody;
    protected Animator animator;
    protected Entity_Stats playerStats;
    protected ScaleData scaleData;
    protected ElementType usedElement;
    protected Color VFXColor = Color.white;
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        skillObjectRigidbody = GetComponent<Rigidbody2D>();
    }
    protected void DamageEnemiesInRadius(Transform transform, float radius, Skill_Base skillManager)
    {
        foreach (var target in GetEnemiesAround(transform, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            ElementalEffectData elementalEffectData = new ElementalEffectData(playerStats, scaleData);

            float physicalDamage = playerStats.GetPhysicalDamage(out bool isCrit, scaleData.physical);
            float elementalDamage = playerStats.GetElementalDamage(out ElementType element, scaleData.elemental);

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(element, elementalEffectData);

            if (targetGotHit)
            {
                lastTarget = target.transform;
                GetVFXHit(skillManager);
            }

            usedElement = element;
        }
    }
    protected Transform ClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
        return target;
    }
    protected Collider2D[] GetEnemiesAround(Transform transform, float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
    }
    protected virtual void OnDrawGizmos()
    {
        if (targetCheckTransform == null)
            targetCheckTransform = transform;

        Gizmos.DrawWireSphere(targetCheckTransform.position, checkRadius);
    }

    protected void GetVFXHit(Skill_Base skillManager)
    {
        SpriteRenderer sprite = Instantiate(VFXPerfab, transform.position, Quaternion.identity).GetComponentInChildren<SpriteRenderer>();
        sprite.color = skillManager.player.playerVFX.UpdateOnHitColor(usedElement, VFXColor);
    }
}
