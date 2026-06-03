using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static playermanajer;
using static UnityEngine.UI.Image;

public class OfficeHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] LocalCamHandler localHandler;
    [SerializeField] SecurityCamHandler CamHandler;
    [SerializeField] DoorHandler DoorHandler;
    [SerializeField] OfficeState CurrentState; //this will be hided in future
    [SerializeField] Transform localTarget;
    [SerializeField] buttonHandler Left, Right, Back;
    [SerializeField] List<GameObject> interactable;
    [SerializeField] List<Vector3> rotation;


    PlayerMain player => PlayerMain.instance;
    int CurRot;

    [Header("variable")]
    public float duration = 1f;

    #region Core ///Core of script///
    private void Start()
    {
        buttonHandling();
    }

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

    void Controller()
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
            //switch (CurrentState)
            //{
            //    case OfficeState.Cam:
            //        StopAllCoroutines();
            //        //StartCoroutine(Cam(false));
            //        RequestSwitchState(OfficeState.Local);
            //        break;
            //    case OfficeState.Door:
            //        StopAllCoroutines();
            //        RequestSwitchState(OfficeState.Local);
            //        //StartCoroutine(camRots(rotation[1]));
            //        break;
            //}
        }

        if (CurrentState == OfficeState.Local)
        {
            if (Right.hover)
            {
                StopAllCoroutines();
                StartCoroutine(camRots(rotation[1]));
                CurRot = 1;
            }
            if (Left.hover)
            {
                StopAllCoroutines();
                StartCoroutine(camRots(rotation[0]));
                CurRot = 0;
            }
        }
    }
    #endregion

    #region methods ///Main Method use in this Script///
    public void RequestSwitchState(OfficeState target)
    {
        disableButton();
        localHandler.enabled = false;
        switch (target)
        {
            case OfficeState.Cam:
                CamHandler.enabled = true;
                DoorHandler.enabled = false;
                localHandler.enabled = false;
                break;
            case OfficeState.Door:
                DoorHandler.enabled = true;
                CamHandler.enabled = false;
                localHandler.enabled = false;
                break;
            case OfficeState.Local:
                localHandler.enabled = true;
                DoorHandler.enabled = false;
                CamHandler.enabled = false;
                break;
        }
        CurrentState = target;
        buttonHandling();
    }

    private void disableButton()
    {
        Left.Active = false;
        Right.Active = false;
        Back.Active = false;
    }
    private void buttonHandling()
    {
        if (CurrentState == OfficeState.Cam || CurrentState == OfficeState.Door)
        {
            Back.Active = true;
        } else Back.Active = false;

        if (CurrentState == OfficeState.Local)
        {
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

                default:
                    Left.Active = false;
                    Right.Active = false;
                    break;
            }
        }
    }

    IEnumerator Cam(bool In) //this thing will moved to cam handler
    {
        disableButton();
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime * duration;
            player.transform.position = Vector3.Lerp(player.transform.position, localTarget.position, t / duration);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, localTarget.rotation, t / duration);
            yield return null;
        }
        buttonHandling();
    }

    IEnumerator camRots(Vector3 target)
    {
        disableButton();
        Quaternion rot = quaternion.Euler(target);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime * duration;
            player.cam.transform.localRotation = Quaternion.Lerp(player.cam.transform.localRotation, rot, t / duration);
            yield return null;
        }
        buttonHandling();
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