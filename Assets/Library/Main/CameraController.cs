using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(-1,1)] public float CameraAngle, CameraTilt;
    [SerializeField] private Vector4 CameraAngleRange; // X= AngleMin, Y = AngleMax, Z = TiltMin, W = TiltMax
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
                Mathf.Lerp(CameraAngleRange.x, CameraAngleRange.y, (CameraAngle + 1f) / 2f),
                Mathf.Lerp(CameraAngleRange.z, CameraAngleRange.w, (CameraTilt + 1f) / 2f)
                );

            cam.transform.localRotation = target;
            yield return new WaitForSeconds(1f / FrameRate);
        }
    }
}
