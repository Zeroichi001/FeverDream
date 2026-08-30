using UnityEngine.UI;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    [Range(0, 1)] public float Blink;
    Material mat;
    Image t;

    private void Awake()
    {
        mat = GetComponent<Image>().material;
    }

    void Update()
    {
        float d = Mathf.Abs(Blink - mat.GetFloat("_BlinkAmount"));

        if (d > 0.001f)
        {
            mat.SetFloat("_BlinkAmount", Blink);
        }
    }
}
