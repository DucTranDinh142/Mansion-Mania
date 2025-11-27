using System;
using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchointerval = .05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCoroutine;

    public void CreateEffectOf(GameObject effect, Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }
    public void DoImageEchoEffect(float duration)
    {
        if(imageEchoCoroutine != null)
            StopCoroutine(imageEchoCoroutine);

        imageEchoCoroutine = StartCoroutine(ImageEchoEffectCoroutine(duration));
    }

    private IEnumerator ImageEchoEffectCoroutine(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchointerval);
            time += imageEchointerval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sprite.sprite;
    }
}
