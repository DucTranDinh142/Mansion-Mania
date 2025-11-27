using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Image fadeImage;
    public Coroutine fadeEffectCoroutine {  get; private set; }

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        //fadeImage.color = new Color(1, 1, 1, 1);
    }
    public void DoFadeIn(float duration = 1.5f)// color => transparent
    {
        fadeImage.color = new Color(1,1,1,1);
        FadeEffect(0f,duration);
    }
    public void DoFadeOut(float duration = 1.5f)//transparent => color
    {
        fadeImage.color = new Color (1,1,1,0);
        FadeEffect(1f,duration);
    }
    private void FadeEffect(float targetAlpha, float duration)
    {
        if (fadeEffectCoroutine != null)
        {
            StopCoroutine(fadeEffectCoroutine);
        }
        fadeEffectCoroutine = StartCoroutine(FadeEffectCoroutine(targetAlpha, duration));
    }

    private IEnumerator FadeEffectCoroutine(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time = time + Time.deltaTime;

            var color = fadeImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeImage.color = color;

            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);

    }
}
