using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Unity.Mathematics;

public class FadeController : MonoBehaviour
{
    [Range(0, 1)] float Alpha;
    public Color BaseColor;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        // Jaga RGB tetap sama, alpha tidak disentuh
        Color c = image.color;
        if (Mathf.Abs(Alpha - c.a) > 0.01f)
        {
            c.a = Alpha;
            image.color = c;
        }

    }

    public IEnumerator FadeIn(float duration, Action onComplete)
    {
        float t = 0f;
        Color c = image.color;

        float startAlpha = c.a;
        float endAlpha = 1f;

        while ((duration - t) > 0.01f)
        {
            t += Time.deltaTime;

            Alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }
        Alpha = endAlpha;
        onComplete?.Invoke();
    }

    public IEnumerator FadeOut(float duration)
    {
        float t = 0f;
        Color c = image.color;

        float startAlpha = c.a;
        float endAlpha = 0f;

        while ((duration - t) > 0.01f)
        {
            t += Time.deltaTime;

            Alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }
        Alpha = endAlpha;
    }
}
