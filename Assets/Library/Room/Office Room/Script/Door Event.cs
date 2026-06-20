using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using System.Collections;
using System.Collections.Generic;

public class DoorEvent : MonoBehaviour
{

    public GameObject door;
    [SerializeField] Vector3 unlocked;

    public bool Occupied;// { get; private set; }
    bool lastState;
    Quaternion Unlocked;

    private void Awake()
    {
        Unlocked = Quaternion.Euler(unlocked);
    }

    void Start()
    {
    //    {
    //        door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation, unlockedRotQ, Time.deltaTime * duration);
    //        if (Quaternion.Angle(door.transform.localRotation, unlockedRotQ) < 0.01f) isOpen = true;
    //    }
    }

    private void Update()
    {
        if (Occupied != lastState)
        {
            if (Occupied)
            {
                door.transform.localRotation = Unlocked;
            }else
            {
                door.transform.localRotation = new Quaternion(0,0,0,0);
            }
        }
    }

    public IEnumerator autoClose(float duration)
    {
        PlayerMain.instance.flashLight.SetActive(false);
        Vector3 rot = new Vector3(0, 0, 0);
        Quaternion closeRotQ = Quaternion.Euler(rot);
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            door.transform.localRotation = Quaternion.Lerp(door.transform.localRotation, closeRotQ, t / duration);
            yield return null;
        }
    }

    public void call()
    {
        Debug.Log("is occupied");
    }
}