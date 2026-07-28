using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCamHandler : MonoBehaviour
{
    // after enable will immedietly move to monitor position
    // every monitor and cam code will be here

    [SerializeField]float duration = 1f;
    [SerializeField]AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public CamData camData = new CamData();

    [Header("Reference")]
    [SerializeField] Transform camTarget;
    [SerializeField] buttonHandler Button1, Button2, Button3, Button4;
    PlayerMain player;

    [SerializeField]RenderTexture RT;

    private void Awake()
    {
        player = PlayerMain.instance;
    }

    private void OnEnable()
    {
        StartCoroutine(In());
        Button1.Active = true;
        Button2.Active = true;
        Button3.Active = true;
        Button4.Active = true;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        RT.Release(); //this should make the render texture black
    }

    private void Update()
    {
        Camera();
    }

    IEnumerator In()
    {
        Quaternion startRot = player.transform.rotation;
        Vector3 startPos = player.transform.position;

        float t = 0f;
        while (t<duration)
            {
                t += Time.deltaTime;
                player.transform.position = Vector3.Lerp(startPos, camTarget.position, curve.Evaluate(t / duration));
                player.transform.rotation = Quaternion.Lerp(startRot, camTarget.rotation, curve.Evaluate(t / duration));
                yield return null;
            }
    }

    void Camera()
    {
        if (Button1.Key)
        {
            camData.Search(CamData.CamType.DiningRoom).gameObject.SetActive(true);
            for (int i = 0; i < camData.camList.Count; i++)
            {
                if (camData.camList[i].type != CamData.CamType.DiningRoom)
                {
                    camData.camList[i].cam.gameObject.SetActive(false);
                }
            }
        }
        if (Button2.Key)
        {
            camData.Search(CamData.CamType.StorageRoom).gameObject.SetActive(true);
            for (int i = 0; i < camData.camList.Count; i++)
            {
                if (camData.camList[i].type != CamData.CamType.StorageRoom)
                {
                    camData.camList[i].cam.gameObject.SetActive(false);
                }
            }
        }
        if (Button3.Key)
        {
            camData.Search(CamData.CamType.Kitchen).gameObject.SetActive(true);
            for (int i = 0; i < camData.camList.Count; i++)
            {
                if (camData.camList[i].type != CamData.CamType.Kitchen)
                {
                    camData.camList[i].cam.gameObject.SetActive(false);
                }
            }
        }
        if (Button4.Key)
        {
            camData.Search(CamData.CamType.EmployeeRoom).gameObject.SetActive(true);
            for (int i = 0; i < camData.camList.Count; i++)
            {
                if (camData.camList[i].type != CamData.CamType.EmployeeRoom)
                {
                    camData.camList[i].cam.gameObject.SetActive(false);
                }
            }
        }
    }

    [System.Serializable]
    public class CamData
    {
        public List<data> camList = new List<data>();

        [System.Serializable]
        public class data
        {
            public CameraController cam;
            public CamType type;
        }

        public enum CamType
        {
            DiningRoom,
            StorageRoom,
            Kitchen,
            EmployeeRoom,
        }

        public CameraController Search(CamType cam) 
        { 
            for (int i = 0; i < camList.Count; i++)
            {
                if (camList[i].type == cam)
                {
                    return camList[i].cam;
                }
            }
            return null;
        }
    }
}
