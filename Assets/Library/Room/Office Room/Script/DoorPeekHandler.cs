using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Transform open, close;
    [SerializeField] float duration = 5f;
    //[SerializeField] float playerDuration = 5f;
    [SerializeField] Vector3 openRot, closeRot;
    [SerializeField] AnimationCurve doorAnimate;
    [SerializeField] AnimationCurve playerAnimate;

    PlayerMain player => PlayerMain.instance;
    DoorEvent Head; //Head Script

    Coroutine Coroutine;
    static bool isQuitting;
    bool isOpen;

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        isQuitting = false;
        Application.quitting += () => isQuitting = true;
    }

    private void Awake()
    {
        if (open == null || close == null) Debug.LogError("Transform var not assigned");
        Head = GetComponent<DoorEvent>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isOpen)
            {
                if (Coroutine != null)
                {
                    StopCoroutine(Coroutine);
                }
                Coroutine = StartCoroutine(doorHandler(false));
                isOpen = false;
            }
        } else if (!isOpen)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            Coroutine = StartCoroutine(doorHandler(true));
            isOpen = true;
        }
    }
    IEnumerator doorHandler(bool isOpen)
    {
        quaternion openRotQ = quaternion.Euler(openRot);
        quaternion closeRotQ = quaternion.Euler(closeRot);

        Quaternion startRot = player.transform.rotation;
        Vector3 startPos = player.transform.position;

        float t = 0f;
        if (!isOpen) player.flashLight.SetActive(false);
        while (t < duration)
        {
            t += Time.deltaTime;

            Head.door.transform.localRotation = Quaternion.Lerp(Head.door.transform.localRotation, isOpen ? openRotQ : closeRotQ, doorAnimate.Evaluate(t / duration));

            player.transform.position = Vector3.Lerp(startPos, isOpen ? open.position : close.position, playerAnimate.Evaluate(t / duration));
            player.transform.rotation = Quaternion.Lerp(startRot, isOpen ? open.rotation : close.rotation, playerAnimate.Evaluate(t / duration));

            yield return null;
        }
        if (isOpen) player.flashLight.SetActive(true);
        Coroutine = null;
    }

    private void OnEnable()
    {
        if (Head.Occupied)
        {
            Head.call();
            this.enabled = false;
        }
        else Coroutine = StartCoroutine(doorHandler(true)); isOpen = true;
    }
    private void OnDisable()
    {
        if (isQuitting) return;
        StopAllCoroutines();
        Coroutine = null;
        StartCoroutine(Head.autoClose(duration));
    }
}
