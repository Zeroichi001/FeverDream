using UnityEngine;

public class CanvasUIManager : MonoBehaviour
{
    public static CanvasUIManager Instance;
    public FadeController FadeC { get; private set; }
    public BlinkController BlinkC { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        FadeC = gameObject.GetComponentInChildren<FadeController>();
        BlinkC = gameObject.GetComponentInChildren<BlinkController>();
    }
}
