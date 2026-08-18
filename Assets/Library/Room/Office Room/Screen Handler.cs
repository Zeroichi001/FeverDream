using System;
using System.Collections;
using UnityEngine;

public class ScreenHandler : MonoBehaviour
{
    public AnomalyData anomalyData = new();

    [System.Serializable]
    public class AnomalyData
    {
        public Anomaly[] anomaly;

        public class Anomaly
        {
            public GameObject Normal;
            public GameObject Anomalous;
        }
    }

    #region Reference

    [SerializeField] GameObject hitBox;
    [SerializeField] float HitBoxRadius;
    [SerializeField] float EyeCloseDuration = 4f;
    [SerializeField] float EyeSpeed = .3f;

    #endregion

    private void Update()
    {
        
    }

    #region Anomaly Detection

    Ray hit;
    IEnumerator HitAnomaly()
    {
        float t = 0;
        bool cancel = false;
        while ((EyeCloseDuration-t) > 0.01f)
        {
            if ((EyeSpeed-t) > 0.01f)
            {
                //do eye close animation step
            }

            if (!Input.GetMouseButtonDown(1))
            {
                cancel = true;
                break;
            }
            t += Time.deltaTime;
            yield return null;
        }

        while (cancel)
        {
            if ((EyeSpeed - t) > 0.01f)
            {
                //do eye open animation step
            }else yield break;

            yield return null;
        }

        Ray hit = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(hit, out RaycastHit rayHit, 10))
        {
            if (rayHit.collider.gameObject != this.gameObject) yield break;
            
            Vector2 UV = rayHit.textureCoord;
            Ray ViewHit = rayHit.collider.gameObject.GetComponentInChildren<Camera>().ViewportPointToRay(UV);

            if (Physics.Raycast(ViewHit, out RaycastHit viewHit, 50))
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
