using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMonitor : MonoBehaviour
{
    [SerializeField] Camera[] cam;
    [SerializeField] Material[] screen;

    public bool isFocused = false;

    bool last = false;
    Renderer rend;
    [SerializeField]int currentCam;

    void Awake()
    {
        rend = gameObject.GetComponent<Renderer>();

        if (screen.Length > 2 || screen.Length < 2 )
        {
            Debug.LogWarning("Camera Monitor on " + gameObject.name + " does not have exactly 2 screen materials assigned.");
        }
    }

    private void Start()
    {
        changeCamera(0);
    }

    private void Update()
    {
        controller();

        if (isFocused != last)
        {
            last = isFocused;

            if (isFocused)
            {
                rend.material = screen[1];
            }
            else
            {
                rend.material = screen[0];
            }
        }
    }

    void controller()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            changeCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            changeCamera(-1);
        }
    }

    void changeCamera(int camera)
    {
        currentCam += camera;
        currentCam = Mathf.Clamp(currentCam, 0, cam.Length - 1);

        for (int i = 0; i < cam.Length; i++)
        {
            if (i == currentCam)
            {
                cam[i].gameObject.SetActive(true);
            }
            else
            {
                cam[i].gameObject.SetActive(false);
            }
        }
    }
}
