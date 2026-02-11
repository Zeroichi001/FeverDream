using UnityEngine;

public class Hover : MonoBehaviour
{
    Outline outline;
    playermanajer pm => playermanajer.instance;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
    }

    private void Update()
    {
        if (pm.hit.collider != null)
        {
            if (pm.hit.collider.gameObject == gameObject)
            {
                outline.enabled = true;
            }
            else outline.enabled = false;
        }
    }
    private void OnDisable()
    {
        outline.enabled = false;
    }
}
