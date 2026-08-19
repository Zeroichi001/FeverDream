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
    [SerializeField] float HitBoxRadius;
    [SerializeField] float EyeCloseDuration = 4f;
    [SerializeField] float EyeSpeed = .3f;

    #endregion

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            StartCoroutine(HitAnomaly());
        }
    }

    #region Anomaly Detection

    IEnumerator HitAnomaly()
    {
        //float t = 0;
        //bool cancel = false;
        //while ((EyeCloseDuration-t) > 0.01f)
        //{
        //    if ((EyeSpeed-t) > 0.01f)
        //    {
        //        //do eye close animation step
        //    }

        //    if (!Input.GetMouseButtonDown(1))
        //    {
        //        cancel = true;
        //        break;
        //    }
        //    t += Time.deltaTime;
        //    yield return null;
        //}

        //while (cancel)
        //{
        //    if ((EyeSpeed - t) > 0.01f)
        //    {
        //        //do eye open animation step
        //    }else yield break;

        //    yield return null;
        //}

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

        //change anomaly state to normal
        //do eye open animation step
        yield return null;
    }

    #endregion
}
