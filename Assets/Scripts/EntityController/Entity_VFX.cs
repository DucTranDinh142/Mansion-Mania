using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sprite;
    private Entity entity;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;

    [Header("On Dealing Damage VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Color critVFXColor = Color.white;
    [SerializeField] private GameObject critVFX;

    [Header("Element Colors")]
    [SerializeField] private Color chillVFXColor = new Color(132f / 255f, 1, 1, 1);
    [SerializeField] private Color burnVFXColor = new Color(1, 170f / 255f, 46f / 255f, 1);
    [SerializeField] private Color shockVFXColor = new Color(1, 254f/255f, 123f / 255f, 1);
    private Color originalHitVFXColor;
    private Color originalCritVFXColor;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sprite.material;
        originalHitVFXColor = hitVFXColor;
        originalCritVFXColor = critVFXColor;
    }
    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCoroutine(chillVFXColor, duration));
        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCoroutine(burnVFXColor, duration));
        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVFXCoroutine(shockVFXColor, duration));
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sprite.color = Color.white;
        sprite.material = originalMaterial;
    }
    private IEnumerator PlayStatusVFXCoroutine(Color effectColor, float duration)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0f;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sprite.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        sprite.color = Color.white;
    }
    public void CreateOnHitVFX(Transform target, bool isCrit, ElementType element)
    {
        GameObject hitPrefab = isCrit ? critVFX : hitVFX;
        Color finalHitColor = isCrit? critVFXColor : hitVFXColor;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = UpdateOnHitColor(element, finalHitColor);

        //if (isCrit == false)
        //    vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
        //else vfx.GetComponentInChildren<SpriteRenderer>().color = critVFXColor;

        if (entity.facingDirectionValue == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }
    public Color UpdateOnHitColor(ElementType element, Color finalHitVFXcolor)
    {
        switch (element)
        {
            case ElementType.Ice:
                return chillVFXColor;
            case ElementType.Fire:
                return burnVFXColor;
            case ElementType.Lightning:
                return shockVFXColor;
            default:
                return finalHitVFXcolor;
        }
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
