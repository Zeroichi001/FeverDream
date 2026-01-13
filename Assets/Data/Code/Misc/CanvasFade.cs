using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FadeController : MonoBehaviour
{
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
        if (c.r != BaseColor.r || c.g != BaseColor.g || c.b != BaseColor.b)
        {
            c.r = BaseColor.r;
            c.g = BaseColor.g;
            c.b = BaseColor.b;
            image.color = c;
        }
    }

    public IEnumerator FadeIn(float duration, Action onComplete)
    {
        float elapsed = 0f;
        Color c = image.color;

        float startAlpha = c.a;
        float endAlpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = c;     // ⭐ WAJIB
            yield return null;
        }

        onComplete?.Invoke();
    }

    public IEnumerator FadeOut(float duration)
    {
        float elapsed = 0f;
        Color c = image.color;

        float startAlpha = c.a;
        float endAlpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = c;     // ⭐ WAJIB
            yield return null;
        }
    }
}
