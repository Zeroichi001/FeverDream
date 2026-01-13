using System.Collections;
using UnityEngine;
using static playermanajer;

public class Door : MonoBehaviour
{
    public bool enableHover; //for hover effect
    public float doorSpeed = 5f; //speed of door open/close

    [SerializeField] private Vector2Int targetPos; //target position when entering the door
    [SerializeField] private bool ispeekDoor; //is this door a peeking door?
    [SerializeField] private bool Flash; //allow flash button
    [SerializeField] private TeleportType Tpy; //teleport method

    private playermanajer pm => playermanajer.instance;
    Player player; //reference to player script
    bool isPeeking = false;
    Coroutine peekingCoroutine;
    Coroutine autoclose;

    void Awake()
    {
        player = Camera.main.GetComponent<Player>();
    }
    private void Update()
    {
        if (pm.currentPos.x == targetPos.x && !isPeeking)
        {
            enableHover = false;
            OnPos();
        } 
        else
        {
            enableHover = true;
        }

        if (enableHover)
        {
            //hover();
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    

    public void select()
    {
        pm.move(targetPos, playermanajer.TeleportType.Walk);
    }

    /// <summary>
    /// Handles the behavior for interacting with a door based on its current state.
    /// </summary>
    private void OnPos()
    {
        if (ispeekDoor)
        {
            if (peekingCoroutine == null)
            {
                if (autoclose != null)
                {
                    StopCoroutine(autoclose);
                }
                peekingCoroutine = StartCoroutine(Peeking());
            }
        }
        else
        {
            peekingCoroutine = StartCoroutine(EnterDoor());
        }
    }

    /// <summary>
    /// peeking door coroutine for peeking type doors
    /// </summary>
    IEnumerator Peeking()
    {
        //local variables
        Quaternion open = Quaternion.Euler(0, 40f, 0);
        Quaternion close = Quaternion.Euler(0, 0, 0);

        while (pm.currentPos.x == targetPos.x)
        {
            //player.FlashButton = Flash; // enable flash button

            //open or close door and disable/enable flashlight
            if (Input.GetKey(KeyCode.Space))
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, close, Time.deltaTime * doorSpeed);
                //player.activeFlashlight = false;
                pm.move(new Vector2Int(targetPos.x, 0), playermanajer.TeleportType.Walk); //move player
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, open, Time.deltaTime * doorSpeed);
                //player.activeFlashlight = true;
                pm.move(new Vector2Int(targetPos.x, -1), playermanajer.TeleportType.Walk); //move player
            }
            yield return null;
        }

        //reset coroutine and run autoclose
        peekingCoroutine = null;
        autoclose = StartCoroutine(AutoClose());
    }

    /// <summary>
    /// autoclose door coroutine after used
    /// </summary>
    IEnumerator AutoClose()
    {
        Quaternion close = Quaternion.Euler(0, 0, 0);
        float timeElapsed = 0f;

        //time counter
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //closing door
        while (Quaternion.Angle(transform.localRotation, close) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, close, Time.deltaTime * doorSpeed);
            //player.activeFlashlight = false;
            //player.FlashButton = false;
            yield return null;
        }
    }

    /// <summary>
    /// entering door coroutine if is not peeking type door
    /// </summary>
    IEnumerator EnterDoor()
    {
        while (pm.currentPos.x == targetPos.x)
        {
            //player.activeFlashlight = true;
            //player.FlashButton = true;
            yield return null;
        }

        //player.activeFlashlight = false;
        //player.FlashButton = false;
        peekingCoroutine = null;
    }
}
