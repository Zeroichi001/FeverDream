using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class OfficeHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] MonoBehaviour localHandler, CamHandler, DoorHandler;
    [SerializeField] OfficeState CurrentState; //this will be hided in future
    [SerializeField] Transform localTarget, camTarget, doorTarget;
    [SerializeField] buttonHandler Left, Right, Back;

    PlayerMain player => PlayerMain.instance;

    [Header("variable")]
    public float duration = 1f;

    #region Core ///Core of script///
    public Quaternion eulerLerp(Vector3 start, Vector3 destination, Vector3 t) //this will be deleted in future
    {
        float x = Mathf.Lerp(start.x, destination.x, t.x) + 360f * (t.x > 1f || t.x < 0f ? t.x - math.trunc(t.x) : 0) * -1;
        float y = Mathf.Lerp(start.y, destination.y, t.y) + 360f * (t.y > 1f || t.y < 0f ? t.y - math.trunc(t.y) : 0) * -1;
        float z = Mathf.Lerp(start.z, destination.z, t.z) + 360f * (t.z > 1f || t.z < 0f ? t.z - math.trunc(t.z) : 0) * -1;

        return Quaternion.Euler(x, y, z);
    }

    public Vector3 posLerp(Vector3 start, Vector3 destination, Vector3 t) //this will be deleted in future
    {
        Vector3 result = new Vector3(
            Mathf.LerpUnclamped(start.x, destination.x, t.x),
            Mathf.LerpUnclamped(start.y, destination.y, t.y),
            Mathf.LerpUnclamped(start.z, destination.z, t.z)
            );
        return result;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StopAllCoroutines();
            StartCoroutine(Cam(true));
            RequestSwitchState(OfficeState.Cam);
            buttonHandling();
        }

        if (Back.hover)
        {
            StopAllCoroutines();
            StartCoroutine(Cam(false));
            RequestSwitchState(OfficeState.Local);
            buttonHandling();
        }
    }
    #endregion

    #region methods ///Main Method use in this Script///
    public void RequestSwitchState(OfficeState target)
    {
        localHandler.enabled = false;
        switch (target)
        {
            case OfficeState.Cam:
                CamHandler.enabled = true;
                DoorHandler.enabled = false;
                break;
            case OfficeState.Door:
                CamHandler.enabled = false;
                DoorHandler.enabled = true;
                break;
        }
        CurrentState = target;
    }

    private void buttonHandling()
    {
        Left.Active = false;
        Right.Active = false;

        if (CurrentState == OfficeState.Cam || CurrentState == OfficeState.Door)
        {
            Back.Active = true;
        } else Back.Active = false;
    }

    IEnumerator Cam(bool In)
    {
        float t = 0f;
        if (!In)
        {
            while (t < duration)
            {
                t += Time.deltaTime;
                player.transform.position = Vector3.Lerp(player.transform.position, localTarget.position, t * duration);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, localTarget.rotation, t * duration);
                yield return null;
            }
        } else if (In)
        {
            while (t < duration)
            {
                t += Time.deltaTime;
                player.transform.position = Vector3.Lerp(player.transform.position, camTarget.position, t * duration);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, camTarget.rotation, t * duration);
                yield return null;
            }
        }
    }
    #endregion  
}

[System.Serializable]
public class curve // this will be deleted in future
{
    public Vector3 position(float t)
    {
        return new Vector3(posX.Evaluate(t), posY.Evaluate(t), posZ.Evaluate(t));
    }
    public Vector3 rotation(float t)
    {
        return new Vector3(rotX.Evaluate(t), rotY.Evaluate(t), rotZ.Evaluate(t));
    }

    public AnimationCurve posX;
    public AnimationCurve posY;
    public AnimationCurve posZ;

    public AnimationCurve rotX;
    public AnimationCurve rotY;
    public AnimationCurve rotZ;

}

[System.Serializable]
public enum OfficeState
{
    Local,
    Cam,
    Door
}