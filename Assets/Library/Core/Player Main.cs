using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class PlayerMain : MonoBehaviour
{
    public static PlayerMain instance;
    [HideInInspector] public Camera cam;
    public GameObject flashLight;
    public CanvasUIManager canvas;

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
}
