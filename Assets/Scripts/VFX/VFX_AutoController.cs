using System.Collections;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sprite;

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Header("Fade effect - Player only, fool")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;
    [Space]
    [Header("Randy Random VFX Rotation")]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;
    [Space]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;
    [Header("Randy Random VFX Position")]
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space]
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCoroutine());
        ApplyRandomOffSet();
        ApplyRandomRotation();

        if(autoDestroy)
            Destroy(gameObject,destroyDelay);
    }
    private IEnumerator FadeCoroutine()
    {
        Color targetColor = Color.white;

        while(targetColor.a > 0)
        {
            targetColor.a -= (fadeSpeed * Time.deltaTime);
            sprite.color = targetColor;
            yield return null;
        }

        sprite.color = targetColor;
    }
    private void ApplyRandomOffSet()
    {
        if (randomOffset == false) return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }
    private void ApplyRandomRotation()
    {
        if(randomRotation == false) return;

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, zRotation);
    }
}
