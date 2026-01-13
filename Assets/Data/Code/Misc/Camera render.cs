using UnityEngine;

public class Camerarender : MonoBehaviour
{
    private Behaviour outline;
    public Camera player => Camera.main;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnWillRenderObject()
    {
        Camera cam = Camera.current;

        if (cam == null) return;

        if (cam == player)
        {
            if (outline.enabled)
            {
                outline.enabled = false;
            }
        }
    }
}
