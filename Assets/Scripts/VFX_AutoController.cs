using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Header("Randy Random VFX Position")]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;
    [Space]
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space]
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    private void Start()
    {
        ApplyRandomOffSet();
        ApplyRandomRotation();

        if(autoDestroy)
            Destroy(gameObject,destroyDelay);
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

        float zRotation = Random.Range(0, 360);
        transform.Rotate(0,0,zRotation);
    }
}
