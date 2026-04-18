using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Transform open, close;
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject door;
    [SerializeField] Vector3 openRot, closeRot, unlocked;

    Player player => Player.instance;

    quaternion openRotQ, closeRotQ, unlockedRotQ;
    public bool Occupied, isOpen;
    public bool State;

    private void Awake()
    {
        //if (door == null) Debug.LogError("Door GameObject not assigned");
        //if (open == null || close == null) Debug.LogError("Transform var not assigned");
    }

    private void Update()
    {
        if (!State)
        {
            if (Occupied)
            {
                doorHandler();
            } else if (isOpen)
            {
                StartCoroutine(autoClose());
            }
        } else
        {
            door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation, unlockedRotQ, Time.deltaTime * duration);
            if (Quaternion.Angle(door.transform.localRotation, unlockedRotQ) < 0.01f) isOpen = true;
        }
    }

    void doorHandler()
    {
        door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation, isOpen ? openRotQ : closeRotQ, Time.deltaTime * duration);

        if (Quaternion.Angle(door.transform.localRotation, closeRotQ) < 0.01f) isOpen = false;
        else if (Quaternion.Angle(door.transform.localRotation, openRotQ) < 0.01f) isOpen = true;

        player.transform.position = Vector3.Lerp(player.transform.position, isOpen ? open.position : close.position, Time.deltaTime * duration);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, isOpen ? open.rotation : close.rotation, Time.deltaTime * duration);
    }

    IEnumerator autoClose()
    {
        while (Quaternion.Angle(door.transform.localRotation, closeRotQ) < 0.01f)
        {
            door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation, closeRotQ, Time.deltaTime * duration);
            yield return null;
        }
        isOpen = false;
    }
}
