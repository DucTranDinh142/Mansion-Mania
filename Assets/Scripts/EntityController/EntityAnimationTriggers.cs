using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }
    protected virtual void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }

    protected virtual void AttackTrigger() 
    {
        entityCombat.PerformAttack();
    }
}
