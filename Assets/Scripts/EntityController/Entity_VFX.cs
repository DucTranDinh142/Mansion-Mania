using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sprite;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;
    [Header("On Dealing Damage VFX")]
    [SerializeField] private Color hitVFXcolor = Color.white;
    [SerializeField] private GameObject hitVFX;
    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sprite.material;
    }
    public void CreateOnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position,Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXcolor;
    }
    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);

        onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    private IEnumerator OnDamageVFXCoroutine()
    {
        sprite.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sprite.material = originalMaterial;
    }
}
