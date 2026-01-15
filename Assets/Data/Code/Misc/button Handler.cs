using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonHandler : MonoBehaviour,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    public bool hover { get; private set; }
    public bool enter { get; private set; }
    public bool Key { get; private set; }
    public bool keyDown { get; private set; }

    bool isPressed;
    bool isInside;

    void Start()
    {
        if(gameObject.GetComponent<Button>() == null)
        {
            Debug.LogWarning("this gameobject doesn't have button component");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressed) return;
        isPressed = true;
        StartCoroutine(click());
        keyDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInside) return;
        isInside = true;
        hover = true;
        StartCoroutine(entering());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressed) return;
        isPressed = false;
        keyDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInside) return;
        isInside = false;
        hover = false;
    }

    IEnumerator click()
    {
        Key = true;
        yield return new WaitForEndOfFrame();
        Key = false;
    }

    IEnumerator entering()
    {
        Debug.Log("enter");
        enter = true;
        //Debug.Log("enter");
        yield return new WaitForEndOfFrame();
        enter = false;
    }

    void LateUpdate()
    {
        if (!hover)
        {
            Key = false;
        }
    }
}