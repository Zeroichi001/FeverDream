using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class OfficeHandler : MonoBehaviour
{
    [Header("Reference")]
    //[SerializeField] LocalCamHandler localHandler;
    [SerializeField] SecurityCamHandler CamHandler;
    [SerializeField] DoorHandler DoorHandler;
    [SerializeField] OfficeState CurrentState; //this will be hided in future
    [SerializeField] Transform localTarget;
    [SerializeField] buttonHandler Left, Right, Back;
    [SerializeField] List<GameObject> interactable;
    [SerializeField] List<Vector3> rotation;
    [SerializeField] AnimationCurve local;
    [SerializeField] AnimationCurve Rotation;
    [SerializeField] AnimationCurve tilt;

    PlayerMain player => PlayerMain.instance;
    [SerializeField] int CurRot;

    [Header("variable")]
    public float duration = 1f;

    #region Core ///Core of script///

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StopAllCoroutines();
            StartCoroutine(Cam(true));
        }

        if (!Application.isFocused) return;
        Controller();
    }

    private void OnEnable() // Turn on every thing needed in this handler //
    {
        buttonManager();
    }

    void Controller() // Method that run Input in this handler // 
    {
        if (Input.GetMouseButton(0))
        {
            switch (interactable.FindIndex(x => x == player.hit.collider?.gameObject))
            {
                case 1:
                    StopAllCoroutines();
                    //StartCoroutine(Cam(true));
                    RequestSwitchState(OfficeState.Cam);
                    break;
                case 2:
                    StopAllCoroutines();
                    RequestSwitchState(OfficeState.Door);
                    break;
            }
        }

        if (Back.hover)
        {
            StopAllCoroutines();
            RequestSwitchState(OfficeState.Local);
            StartCoroutine(Cam(false));
        }

        if (CurrentState == OfficeState.Local)
        {
            if (Right.hover)
            {
                StopAllCoroutines();
                StartCoroutine(camRots(rotation[1], true));
                CurRot = 1;
            }
            if (Left.hover)
            {
                StopAllCoroutines();
                StartCoroutine(camRots(rotation[0], false));
                CurRot = 0;
            }
        }
    }
    #endregion

    #region methods ///Main Method use in this Script///
    public void RequestSwitchState(OfficeState target)
    {
        disableButton();
        //localHandler.enabled = false;
        switch (target)
        {
            case OfficeState.Cam:
                CamHandler.enabled = true;
                DoorHandler.enabled = false;
                //localHandler.enabled = false;
                break;
            case OfficeState.Door:
                DoorHandler.enabled = true;
                CamHandler.enabled = false;
                //localHandler.enabled = false;
                break;
            case OfficeState.Local:
                //localHandler.enabled = true;
                DoorHandler.enabled = false;
                CamHandler.enabled = false;
                break;
        }
        CurrentState = target;
        buttonManager();
    }

    private void disableButton()
    {
        Left.Active = false;
        Right.Active = false;
        Back.Active = false;
    }

    private void buttonManager()
    {
        if (CurrentState == OfficeState.Local)
        {
            switch (CurRot)
            {
                case 0:
                    Left.Active = false;
                    Right.Active = true;
                    Back.Active = false;
                    break;
                case 1:
                    Left.Active = true;
                    Right.Active = false;
                    Back.Active = false;
                    break;
            }
        }
        else
        {
            Left.Active = false;
            Right.Active = false;
            Back.Active = true;
        }
    }

    IEnumerator Cam(bool In) //this thing will moved to cam handler
    {
        disableButton();
        float t = 0f;
        Quaternion startRot = player.transform.rotation;
        Vector3 startPos = player.transform.position;
        while (t < duration)
        {
            t += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPos, localTarget.position, local.Evaluate(t / duration));
            player.transform.rotation = Quaternion.Lerp(startRot, localTarget.rotation, local.Evaluate(t / duration));
            yield return null;
        }
        buttonManager();
    }

    IEnumerator camRots(Vector3 target, bool right)
    {
        disableButton();
        Quaternion rot = Quaternion.Euler(0, target.y, 0);
        float t = 0f;
        Quaternion startRot = player.cam.transform.localRotation;

        while (t < duration)
        {
            t += Time.deltaTime;
            quaternion currentRot = Quaternion.Lerp(startRot, rot, Rotation.Evaluate(t / duration));
            float tiltValue = (5f * (right ? 1 : -1) * tilt.Evaluate(t / duration));

            player.cam.transform.localRotation = currentRot * Quaternion.Euler(0, 0, tiltValue);
            yield return null;
        }
        switch (CurRot)
        {
            case 0:
                Left.Active = false;
                Right.Active = true;
                break;
            case 1:
                Left.Active = true;
                Right.Active = false;
                break;
        }
    }
    #endregion  
}

[System.Serializable]
public enum OfficeState
{
    Local,
    Cam,
    Door
}