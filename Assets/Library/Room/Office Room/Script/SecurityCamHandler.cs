using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCamHandler : MonoBehaviour
{
    // after enable will immedietly move to monitor position
    // every monitor and cam code will be here

    [SerializeField]float duration = 1f;
    [SerializeField]AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Reference")]
    [SerializeField] Transform camTarget;
    PlayerMain player;

    private void Awake()
    {
        player = PlayerMain.instance;
    }

    private void OnEnable()
    {
        StartCoroutine(In());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
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
}
