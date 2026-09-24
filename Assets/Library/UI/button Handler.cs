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

    CanvasGroup canvasGroup => GetComponent<CanvasGroup>();

    bool isPressed;
    bool isInside;

    public bool Active = true;
    public bool Muted = false;
    [Range(0,1)]public float Alpha = 1f;

    void Start()
    {
        if(gameObject.GetComponent<Button>() == null)
        {
            Debug.LogWarning("this gameobject doesn't have button component");
        }
        if (canvasGroup == null)
        {
            Debug.LogWarning("this gameobject doesn't have CanvasGroup component");
        }

        actived();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Muted) return;
        if (isPressed) return;
        isPressed = true;
        StartCoroutine(click());
        keyDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Muted) return;
        if (isInside) return;
        isInside = true;
        hover = true;
        StartCoroutine(entering());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Muted) return;
        if (!isPressed) return;
        isPressed = false;
        keyDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Muted) return;
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
        enter = true;
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

    bool _Active;
    bool _Muted;
    void actived()
    {
        _Active = Active;
        switch (Active)
        {
            case true:
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                break;
            case false:
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                break;
        }
    }

    private void Update()
    {
        if (_Active != Active)
        {
            actived();
        }

        if (_Active)
        {
            canvasGroup.alpha = Alpha;
        }

        if (!Muted && _Muted == Muted) return;
        isInside = false;
        isPressed = false;
        
        keyDown = false;
        Key = false;
        hover = false;
        enter = false;

        _Muted = Muted;
    }
}