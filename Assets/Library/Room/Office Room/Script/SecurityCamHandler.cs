using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCamHandler : MonoBehaviour
{
    // after enable will immedietly move to monitor position
    // every monitor and cam code will be here

    public class Variables
    {
        public CamData.CamType currentCam;
    } public Variables variables = new();

    [SerializeField]float duration = 1f;
    [SerializeField]AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField]AnimationCurve noiseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public CamData camData = new CamData();
    [Range(0,1)] public float NoiseIntensity = 0;

    [Header("Reference")]
    [SerializeField] Transform camTarget;
    [SerializeField] buttonHandler Button1, Button2, Button3, Button4;
    [SerializeField] GameObject Screen;
    Collider screenCol => gameObject.GetComponent<Collider>();
    Material screenMat;
    PlayerMain player;

    [SerializeField]RenderTexture RT;

    private void Awake()
    {
        player = PlayerMain.instance;
        screenMat = Screen.GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        StartCoroutine(In());

        screenCol.enabled = false;

        Button1.Active = true;
        Button2.Active = true;
        Button3.Active = true;
        Button4.Active = true;
        
        Button1.Muted = false;
        Button2.Muted = false;
        Button3.Muted = false;
        Button4.Muted = false;
    }
    private void OnDisable()
    {
        screenCol.enabled = true;

        Button1.Active = false;
        Button2.Active = false;
        Button3.Active = false;
        Button4.Active = false;

        StopAllCoroutines();

        for (int i = 0; i < camData.camList.Count; i++)
        {
            if (!camData.camList[i].cam.gameObject.activeInHierarchy) return;
            camData.camList[i].cam.gameObject.SetActive(false);
        }
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

    void monitor()
    {

    }

    void Camera()
    {
        if (Button1.Key)
        {
            StartCoroutine(CamChange(CamData.CamType.DiningRoom));
        }
        if (Button2.Key)
        {
            StartCoroutine(CamChange(CamData.CamType.StorageRoom));
        }
        if (Button3.Key)
        {
            StartCoroutine(CamChange(CamData.CamType.Kitchen));
        }
        if (Button4.Key)
        {
            StartCoroutine(CamChange(CamData.CamType.EmployeeRoom));
        }
    }

    [SerializeField] float SwitchDuration = 1f;
    IEnumerator CamChange(CamData.CamType camType)
    {
        float _duration;
        if (variables.currentCam != camType)
        {
            _duration = Random.Range(0f, 3f);
            variables.currentCam = camType;
        } else _duration = 0f;

        float t = 0f;
        Button1.Muted = true;
        Button2.Muted = true;
        Button3.Muted = true;
        Button4.Muted = true;

        while (SwitchDuration - t > 0.01f)
        {
            t += Time.deltaTime;
            screenMat.SetFloat("_NoiseIntensity", Mathf.Lerp(0, 1, noiseCurve.Evaluate(t / duration)));
            yield return null;
        }

        for (int i = 0; i < camData.camList.Count; i++)
        {
            if (camData.camList[i].type == camType)
            {
                camData.camList[i].cam.gameObject.SetActive(true);
            }
            else
            {
                camData.camList[i].cam.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(_duration);

        t = 0f;
        while (SwitchDuration - t > 0.01f)
        {
            t += Time.deltaTime;
            screenMat.SetFloat("_NoiseIntensity", Mathf.Lerp(1, 0, noiseCurve.Evaluate(t / duration)));
            yield return null;
        }
        
        Button1.Muted = false;
        Button2.Muted = false;
        Button3.Muted = false;
        Button4.Muted = false;

        yield return null;
    }

    [System.Serializable]
    public class CamData
    {
        public List<data> camList = new List<data>();

        [System.Serializable]
        public class data
        {
            public CameraController cam;
            public Camera camRef => cam.GetComponentInChildren<Camera>();
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
