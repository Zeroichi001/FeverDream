using System;
using System.Collections;
using UnityEngine;
using static SecurityCamHandler;
using static SecurityCamHandler.CamData;

public class ScreenHandler : MonoBehaviour
{
    #region Reference & Variables

    [SerializeField] GameObject hitBox;
    [SerializeField] SecurityCamHandler Main;
    [SerializeField] float EyeCloseDuration = 4f;
    [SerializeField] float EyeSpeed = .3f;
    [SerializeField] float DragSpeed = 1f;

    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && Detect == null)
        {
            Detect = StartCoroutine(HitAnomaly());
        }

        if (Input.GetMouseButtonDown(0) && Drag == null)
        {
            Drag = StartCoroutine(CameraControl());
        }
    }

    #region Anomaly Detection

    Coroutine Detect;
    IEnumerator HitAnomaly()
    {
        Debug.Log("start");

        Ray hit = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(hit, out RaycastHit rayHit, 10))
        {
            if (rayHit.collider.gameObject != this.gameObject) yield break;

            Vector2 UV = rayHit.textureCoord;
            Ray ViewHit = new Ray();

            for (int i = 0; i < Main.camData.camList.Count; i++)
            {
                if (Main.camData.camList[i].type == Main.variables.currentCam)
                {
                    ViewHit = Main.camData.camList[i].camRef.ViewportPointToRay(UV);
                }
            }

            if (Physics.Raycast(ViewHit, out RaycastHit viewHit, 100))
            {
                hitBox.SetActive(true);
                hitBox.transform.position = viewHit.point;
            }
        } //Raycast to Viewport scene & place Hitbox

        float t = 0;
        bool cancel = false;
        while ((EyeCloseDuration - t) > 0.01f)
        {
            if ((EyeSpeed - t) > 0.01f)
            {
                //do eye close animation step
            }

            if (!Input.GetMouseButton(1))
            {
                cancel = true;
                break;
            }
            t += Time.deltaTime;
            yield return null;
        }//start progress & canceling trigger here

        while (cancel)
        {
            Debug.Log("cancel");
            if (t > 0.01f)
            {
                //do eye open animation step
                t -= Time.deltaTime;
            } else
            {
                Detect = null;
                yield break;
            }

            yield return null;
        }//cancel loop
        Debug.Log("detected");

        AnomalyManager.Instance.anomalyData.ClearAnomaly(Main.variables.currentCam, hitBox.GetComponent<HitBox>().DetectedObject);//change anomaly state to normal

        Debug.Log("done");
        GameObject test = hitBox.GetComponent<HitBox>().DetectedObject;
        Debug.Log(test);
        t = 0f;
        while (true)
        {
            if (t > 0.01f)
            {
                //do eye open animation step
                t += Time.deltaTime;
            }
            else break;

            yield return null;
        }//do eye open animation step
        Detect =null;
    }

    #endregion

    #region Camera Drag Control

    Coroutine Drag;
    IEnumerator CameraControl()
    {
        Ray Start = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 start = new Vector2(0,0);
        Vector2 end = new Vector2(0,0);

        

        if (Physics.Raycast(Start, out RaycastHit rayHit, 10))
        {
            if (rayHit.collider.gameObject != this.gameObject)
            {
                Drag = null;
                yield break;
            }

            start = rayHit.textureCoord;
        }

        CameraController camera = Main.camData.Search(Main.variables.currentCam);

        while (true)
        {
            if (!Input.GetMouseButton(0)) break;

            Ray End = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(End, out RaycastHit rayHit0, 10))
            {
                if (rayHit0.collider.gameObject != this.gameObject) break;

                end = rayHit0.textureCoord;
            }

            Vector2 delta = end - start;
            camera.CameraAngle += delta.x * DragSpeed;
            camera.CameraTilt -= delta.y * DragSpeed;

            start = end;
            yield return null;
        }
        Drag = null;
    }

    #endregion
}
