using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LightBlink : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float BlinkAmount;
    [SerializeField, Range(0f, 1f)] private float BlinkTime;
    [SerializeField, Range(1, 60)] private int Frame;
    [SerializeField] GameObject Light;
    bool off;

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float t = 0f;
        while (true)
        {
            if (Random.Range(0, 1f) < BlinkAmount && !off)
            {
                off = true;
                Light.SetActive(false);
            }
            else if (off && (t / BlinkTime) < 0.01f)
            {
                off = false;
                Light.SetActive(true);
            }
            yield return new WaitForSeconds(1f / Frame);
        }
    }
}
