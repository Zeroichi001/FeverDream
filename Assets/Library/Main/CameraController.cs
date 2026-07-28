using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(-1,1)] public float CameraAngle;
    [SerializeField] private GameObject cam;
    [SerializeField] private int FrameRate = 12;
    public Camera local { get; private set; }

    private void Awake()
    {
        local = cam.GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        StartCoroutine(TickUpdate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TickUpdate()
    {
        while (true)
        {
            Quaternion target = Quaternion.Euler(
                cam.transform.localEulerAngles.x,
                CameraAngle * 35f,
                cam.transform.localEulerAngles.z
                );

            cam.transform.localRotation = target;
            yield return new WaitForSeconds(1f / FrameRate);
        }
    }
}
