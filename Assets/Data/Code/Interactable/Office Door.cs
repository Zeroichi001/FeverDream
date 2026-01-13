using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class OfficeDoor : MonoBehaviour
{
    playermanajer pm => playermanajer.instance;
    Player player => Player.instance;

    Hover hover;
    [SerializeField] private Vector2Int targetPos; //target position when entering the door
    Coroutine peek;
    Coroutine close;

    private void Start()
    {
        hover = gameObject.GetComponent<Hover>();
        checking();
    }

    private void Update()
    {
        if (pm.hit.collider !=null && pm.raycastEnabled)
        {
            if (pm.hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    select();
                }
            }
        }
        status();
    }

    void checking()
    {
        if (hover == null)
        {
            Debug.LogError("Hover Component not found");
        }
        if (gameObject.GetComponent<Collider>() == null)
        {
            Debug.LogError("No collider found");
        }
    }

    //main function==================================================================================>

    void status()
    {
        if (pm.currentPos.x == targetPos.x)
        {
            if (peek == null)
            {
                peek = StartCoroutine(Peeking());
                player.SwitchMethod(cameramethod.flashligt, MethodParam.Default);
            }
            if (close != null)
            {
                StopCoroutine(close);
                close = null;
            }
        }
        else
        {
            if (peek != null)
            {
                StopCoroutine(peek);
                peek = null;
            }
            if (close == null)
            {
                close = StartCoroutine(AutoClose());
            }
        }
    }

    IEnumerator Peeking()
    {
        //local variables
        Quaternion open = Quaternion.Euler(0, 40f, 0);
        Quaternion close = Quaternion.Euler(0, 0, 0);
        float doorSpeed = 5f;
        //player.SwitchMethod(cameramethod.flashligt, MethodParam.Default);

        while (pm.currentPos.x == targetPos.x)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, close, Time.deltaTime * doorSpeed);
                pm.move(new Vector2Int(targetPos.x, 0), playermanajer.TeleportType.Walk);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, open, Time.deltaTime * doorSpeed);
                pm.move(new Vector2Int(targetPos.x, -1), playermanajer.TeleportType.Walk);
            }
            yield return null;
        }

    }

    IEnumerator AutoClose()
    {
        Quaternion close = Quaternion.Euler(0, 0, 0);
        float doorSpeed = 5f;
        float timeElapsed = 0f;

        if (pm.currentPos.x > 1)
        {
            while (timeElapsed < pm.fadeDuration)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        //closing door
        while (Quaternion.Angle(transform.localRotation, close) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, close, Time.deltaTime * doorSpeed);
            yield return null;
        }
    }

    //other function================================================================================>

    public void select()
    {
        if (pm.currentPos.x > 1)
        {
            pm.move(targetPos, playermanajer.TeleportType.Fade);
        }
        else if (pm.currentPos.x < 1)
        {
            pm.move(targetPos, playermanajer.TeleportType.Walk);
        }
    }
}
