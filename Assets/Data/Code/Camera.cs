using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float cameraSpeed = 10f; //camera follow up speed
    public GameObject flashlight; //reference to flashlight
    public static Player instance;

    [HideInInspector] public Coroutine camCourotine; //camera method

    public buttonHandler buttonbottom;
    public buttonHandler buttonleft;
    public buttonHandler buttonright;

    float targetYaw; //save target yaw
    float targetPitch; //save target pitch
    playermanajer pm; //reference to player manager
    Camera cam => Camera.main; //reference to main camera

    public enum cameramethod
    {
        swipe,
        freelook,
        flashligt
    }

    public struct MethodParam
    {
        public List<Vector3> LookEuler;
        public int lookDefault;
        public int lastLook;
        public float maxYaw;
        public float maxPitch;

        public static MethodParam Default => new MethodParam
        {
            LookEuler = new List<Vector3>()
        };
    }

    private void Awake()
    {
        instance = this;
        pm = GameObject.FindGameObjectWithTag("Player Manajer").GetComponent<playermanajer>();
    }

    private void Start()
    {
        
    }

    //main function==================================================================================>

    public void SwitchMethod(cameramethod method, object param = null)
    {
        if (camCourotine != null)
        {
            StopCoroutine(camCourotine);
            camCourotine = null;
        }

        StartCoroutine(resetCam());

        switch(method)
        {
            case cameramethod.swipe:
                camCourotine = StartCoroutine(Swipecam((MethodParam)param));
                break;
            case cameramethod.freelook:
                camCourotine = StartCoroutine(FreeCam((MethodParam)param));
                break;
            case cameramethod.flashligt:
                camCourotine = StartCoroutine(Flashlight((MethodParam)param));
                break;
        }
    }
    IEnumerator resetCam()
    {
        Quaternion Default = Quaternion.Euler(new Vector3(0,0,0));

        while (Quaternion.Angle(transform.localRotation, Default) > 0.01f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Default, Time.deltaTime * cameraSpeed);
            yield return null;
        }
    }

    IEnumerator Flashlight(MethodParam param)
    {
        while (true)
        {
            if (buttonbottom.hover)
            {
                flashlight.SetActive(true);
            } else
            {
                flashlight.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator Swipecam(MethodParam param)
    {
        List<Quaternion> rotation = new List<Quaternion>();
        foreach (var rotate in param.LookEuler)
        {
            rotation.Add(Quaternion.Euler(rotate));
        }

        int last;
        int current;

        if (param.lastLook == -1)
        {
            current = param.lookDefault;
        } else
        {
            current = param.lastLook;
        }

        while (Quaternion.Angle(transform.localRotation, rotation[current]) > 0.01f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation[current], Time.deltaTime * cameraSpeed);
            yield return null;
        }

        last = current;

        while (true)
        {
            if (buttonleft.enter)
            {
                if (current > 0)
                {
                    current--;
                }
            }
            if (buttonright.enter)
            {
                if (current < (rotation.Count - 1))
                {
                    current++;
                }
            }

            if (current != last)
            {
                while (Quaternion.Angle(transform.localRotation, rotation[current]) > 0.01f)
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation[current], Time.deltaTime * cameraSpeed);
                    yield return null;
                }
                last = current;
            }

            yield return null;
        }

    }

    IEnumerator FreeCam (MethodParam param)
    {
        while (true)
        {
            // Ambil posisi kursor relatif terhadap lebar & tinggi layar (0 - 1)
            float mouseX = Input.mousePosition.x / Screen.width;
            float mouseY = Input.mousePosition.y / Screen.height;

            // Konversi ke -1 sampai 1 (tengah = 0)
            float normalizedX = Mathf.Clamp((mouseX - 0.5f) * 2f, -1f, 1f);
            float normalizedY = Mathf.Clamp((mouseY - 0.5f) * 2f, -1f, 1f);

            // Tentukan target rotasi berdasarkan posisi kursor
            targetYaw = normalizedX * param.maxYaw;
            targetPitch = -normalizedY * param.maxPitch;

            //Debug.Log(new Vector2(normalizedX, normalizedY));
            // Smooth rotasi (biar gak patah-patah)
            Quaternion target = Quaternion.Euler(targetPitch, targetYaw, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * cameraSpeed);
            yield return null;
        }
    }

    void FlashLight ()
    {
        if (!flashlight.activeInHierarchy)
        {
            flashlight.SetActive(true);
        }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.origin + ray.direction * 10f;

        flashlight.transform.LookAt(targetPoint);
    }

    //other function================================================================================>

}
