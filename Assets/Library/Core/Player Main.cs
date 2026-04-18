using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class PlayerMain : MonoBehaviour
{
    public static PlayerMain instance;
    public Camera cam {  get; private set; }
    public GameObject flashLight;
    public CanvasUIManager canvas;

    [HideInInspector] public bool raycastEnabled = true;
    [HideInInspector] public RaycastHit hit;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cam = Camera.main;
        if (cam == null) Debug.LogError("Main Camera not found in the scene.");
        if (flashLight == null) Debug.LogError("Flashlight not assigned in PlayerMain script.");
    }

    private void Update()
    {
        if (!Application.isFocused) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (raycastEnabled)
        {
            Physics.Raycast(ray, out hit, 30f);
        }
    }
}
