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

    public Coroutine camCourotine { get; private set; } //camera method
    public cameramethod currentCamMethod { get; private set; }
    public Parameter sharedParam;

    public buttonHandler buttonbottom;
    public buttonHandler buttonleft;
    public buttonHandler buttonright;

    float targetYaw; //save target yaw
    float targetPitch; //save target pitch
    playermanajer pm => playermanajer.instance; //reference to player manager
    Camera cam => Camera.main; //reference to main camera

    public enum cameramethod
    {
        swipe,
        freelook,
        flashligt
    }

    public class Parameter
    {
        public State state = new State();
        public Var var = new Var();

        public class State
        {
            //Swipe state
            public int currentRot;

            //flashlight state
            public bool isOpen;
        }

        public struct Var
        {
            //Swipe var
            public List<Vector3> LookEuler;
            public int lookDefault;
            public int lastLook;

            //freelook var
            public float maxYaw;
            public float maxPitch;
        }

        public static Parameter Default => new Parameter
        {
            var = new Var
            {
                LookEuler = new List<Vector3>(),
            }
        };

        public void resetState()
        {
            
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    #region Main Functions

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
                camCourotine = StartCoroutine(Swipecam((Parameter)param));
                currentCamMethod = cameramethod.swipe;
                break;
            case cameramethod.freelook:
                camCourotine = StartCoroutine(FreeCam((Parameter)param));
                currentCamMethod = cameramethod.freelook;
                break;
            case cameramethod.flashligt:
                camCourotine = StartCoroutine(Flashlight((Parameter)param));
                currentCamMethod = cameramethod.flashligt;
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

    IEnumerator Flashlight(Parameter param)
    {
        sharedParam = param;
        while (true)
        {
            if (param.state.isOpen)
            {
                flashlight.SetActive(true);
            } else
            {
                flashlight.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator Swipecam(Parameter param)
    {
        sharedParam = param;

        List<Quaternion> rotation = new List<Quaternion>();
        foreach (var rotate in param.var.LookEuler)
        {
            rotation.Add(Quaternion.Euler(rotate));
        }

        int last = -1;

        param.state.currentRot = param.var.lastLook == -1
            ? param.var.lookDefault
            : param.var.lastLook;

        while (true)
        {
            if (buttonleft.enter && param.state.currentRot > 0) param.state.currentRot--;
            if (buttonright.enter && param.state.currentRot < (rotation.Count - 1)) param.state.currentRot++;

            if (param.state.currentRot != last)
            {
                while (Quaternion.Angle(transform.localRotation, rotation[param.state.currentRot]) > 0.01f)
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation[param.state.currentRot], Time.deltaTime * cameraSpeed);
                    yield return null;
                }
                last = param.state.currentRot;
            }
            yield return null;
        }
    }

    IEnumerator FreeCam (Parameter param)
    {
        sharedParam = param;
        while (true)
        {
            // Ambil posisi kursor relatif terhadap lebar & tinggi layar (0 - 1)
            float mouseX = Input.mousePosition.x / Screen.width;
            float mouseY = Input.mousePosition.y / Screen.height;

            // Konversi ke -1 sampai 1 (tengah = 0)
            float normalizedX = Mathf.Clamp((mouseX - 0.5f) * 2f, -1f, 1f);
            float normalizedY = Mathf.Clamp((mouseY - 0.5f) * 2f, -1f, 1f);

            // Tentukan target rotasi berdasarkan posisi kursor
            targetYaw = normalizedX * param.var.maxYaw;
            targetPitch = -normalizedY * param.var.maxPitch;

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
    #endregion
}
