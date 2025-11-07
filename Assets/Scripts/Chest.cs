using UnityEngine;

public class Chest : MonoBehaviour , IDamagable
{
    private Rigidbody2D chestRigidbody2D => GetComponentInChildren<Rigidbody2D>();
    private Animator animator => GetComponentInChildren<Animator>();
    private Entity_VFX VFX => GetComponentInChildren<Entity_VFX>();
    public void TakeDamage(float damage, Transform damageDealer)
    {
        VFX.PlayOnDamageVFX();
        animator.SetBool("Open", true);
        chestRigidbody2D.linearVelocity = new Vector2(0, 5);
        chestRigidbody2D.angularVelocity = Random.Range(-150f, 150f);
    }
}
