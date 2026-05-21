using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Transform open, close;
    [SerializeField] float duration = 5f;
    [SerializeField] Vector3 openRot, closeRot;

    Player player => Player.instance;
    DoorEvent Head; //Head Script

    quaternion openRotQ, closeRotQ;
    public bool isOpen;
    static bool isQuitting;

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

        openRotQ = quaternion.Euler(openRot);
        closeRotQ = quaternion.Euler(closeRot);

    }

    private void Update()
    {
        doorHandler();
    }

    void doorHandler()
    {
        Head.door.transform.localRotation = Quaternion.Lerp(Head.door.transform.localRotation, isOpen ? openRotQ : closeRotQ, Time.deltaTime / duration);

        if (Quaternion.Angle(Head.door.transform.localRotation, closeRotQ) < 0.01f) isOpen = false;
        else if (Quaternion.Angle(Head.door.transform.localRotation, openRotQ) < 0.01f) isOpen = true;

        player.transform.position = Vector3.Lerp(player.transform.position, isOpen ? open.position : close.position, Time.deltaTime / duration);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, isOpen ? open.rotation : close.rotation, Time.deltaTime / duration);
    }

    private void OnEnable()
    {
        if (Head.Occupied)
        {
            Head.call();
            this.enabled = false;
        }
        else return;
    }
    private void OnDisable()
    {
        if (isQuitting) return;
        StartCoroutine(Head.autoClose(duration));
    }
}
