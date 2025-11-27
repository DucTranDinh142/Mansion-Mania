using UnityEngine;

public class Object_Chest : MonoBehaviour , IDamagable
{
    private Rigidbody2D chestRigidbody2D => GetComponentInChildren<Rigidbody2D>();
    private Animator animator => GetComponentInChildren<Animator>();
    private Entity_VFX VFX => GetComponentInChildren<Entity_VFX>();
    private Entity_DropManager dropManager => GetComponent<Entity_DropManager>();

    [SerializeField] private bool canDropItems = true;
    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (canDropItems == false) return false;
        dropManager?.DropItems();
        canDropItems = false;
        VFX.PlayOnDamageVFX();
        animator.SetBool("Open", true);
        chestRigidbody2D.linearVelocity = new Vector2(0, 5);
        chestRigidbody2D.angularVelocity = Random.Range(-150f, 150f);

        return true;
    }
}
