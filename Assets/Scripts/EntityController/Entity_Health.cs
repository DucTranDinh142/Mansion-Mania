using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHP;
    public bool isDead {  get; private set; }
    protected bool canTakeDamage = true;
    [Header("HP Regen")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHP = true;
    public float lastDamageTaken;
    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7f, 7f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    [Header("On Heavy Damage")]
    [Range(0f, 1f)]
    [SerializeField] private float heavyDamageThreshold = 0.3f;
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
        SetUpHealth();

    }

    private void SetUpHealth()
    {
        if (entityStats == null) return;
            currentHP = entityStats.GetMaxHP();
            UpdateHealthBar();
            InvokeRepeating(nameof(RegenerateHP), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false) return false;

        if (AttackEvaded()) return false;

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorRedution() : 0f;


        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0f;
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats != null ? entityStats.GetElementalReistance(element) : 0f;
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockBack(damageDealer, physicalDamageTaken);
        ReduceHP(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;

        return true;
    }

    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;

    private bool AttackEvaded()
    {
        if (entityStats == null) return false;
        else 
            return Random.Range(0, 100) < entityStats.GetEvasion();
    }

    public void IncreaseHP(float healAmount)
    {
        if (isDead) return;

        float newHP = currentHP + healAmount;
        float maxHP = entityStats.GetMaxHP();

        currentHP = Mathf.Min(newHP, maxHP);
        UpdateHealthBar();

    }
    public void RegenerateHP()
    {
        if (canRegenerateHP == false) return;

        float regenAmount = entityStats.resourceStat.healthRegen.GetValue();
        IncreaseHP(regenAmount);
    }
    public void ReduceHP(float damage, bool statusVFX = true)
    {
        if (statusVFX)
            entityVFX?.PlayOnDamageVFX();

        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
    }
    public float GetHPPercent() => (float)currentHP / entityStats.GetMaxHP();
    public void SetHPtoPercent(float percent)
    {
        currentHP = entityStats.GetMaxHP() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.value = currentHP / entityStats.GetMaxHP();
    }
    private void TakeKnockBack(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.RecieveKnockback(knockback, duration);
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x *= direction;

        return knockback;
    }
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null) return false;
        else return
            damage / entityStats.GetMaxHP() > heavyDamageThreshold;

    }
        
}
