using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCamHandler : MonoBehaviour
{
    // after enable will immedietly move to monitor position
    // every monitor and cam code will be here

    [SerializeField]float duration = 1f;

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
        float t = 0f;
        while (t<duration)
            {
                t += Time.deltaTime;
                player.transform.position = Vector3.Lerp(player.transform.position, camTarget.position, t / duration);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, camTarget.rotation, t / duration);
                yield return null;
            }
    }
}
