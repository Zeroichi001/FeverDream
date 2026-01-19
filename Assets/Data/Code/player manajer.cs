using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class playermanajer : MonoBehaviour
{
    public static playermanajer instance;

    [Header("Setting")]
    public float Movespeed = 10f;
    public bool raycastEnabled = true;
    public GameObject player; //reference to player

    [HideInInspector] public Camera cam => Camera.main;
    [HideInInspector] public RaycastHit hit;

    public Vector2Int currentPos;// { get; private set; } //current position of the player
    [Range(0.5f, 1f)] public float fadeDuration { get; private set; } //fade duration
    public bool moving { get; private set; }
    [SerializeField] List<Transform> pos; //list of available positions

    Ray ray;
    Vector2Int lastPos = new Vector2Int(0 ,-1);
    TeleportType lastTeleportType;
    Coroutine movingCoroutine;
    FadeController Fc;
    Player cameraPlayer => Player.instance;

    public Transform locRequest(Vector2Int location)
    {
        if (location.y == -1)
        {
            return pos[location.x];
        }
        else
        {
            Transform target = pos[location.x].GetChild(location.y);
            return target;
        }
    }

    public enum TeleportType
    {
        Walk,
        Fade
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        move(new Vector2Int(0,-1), TeleportType.Walk);
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        Fc = canvas.GetComponentInChildren<FadeController>();
    }

    private void Update()
    {
        if (!Application.isFocused) return;
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (raycastEnabled)
        {
            Physics.Raycast(ray, out hit, 30f);
        }
        controller();
    }

    //main function==================================================================================>

    /// <summary>
    /// player controller
    /// </summary>
    /// <remarks> controlling player input</remarks>
    void controller()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            move(lastPos, lastTeleportType);
        }
        if (cameraPlayer.buttonbottom.enter)
        {
            move(lastPos, lastTeleportType);
        }
    }

    //other function==================================================================================>

    /// <summary>
    /// move request for the player
    /// </summary>
    public void move(Vector2Int target, TeleportType method)
    {
        if (currentPos != target)
        {
            Transform locate = locRequest(target);

            if (movingCoroutine != null)
            {
                StopCoroutine(movingCoroutine);
            }

            switch (method)
            {
                case TeleportType.Walk:
                    movingCoroutine = StartCoroutine(Walk(locate));
                    break;
                case TeleportType.Fade:
                    movingCoroutine = StartCoroutine(Teleport(locate));
                    break;
            }
            lastTeleportType = method;

            if (currentPos.x != target.x)
            {
                lastPos.x = currentPos.x;
            }
            currentPos = target;
        }
    }

    IEnumerator Walk (Transform target)
    {
        moving = true;
        while (Vector3.Distance(player.transform.position, target.position) > 0.01f || 
            Quaternion.Angle(player.transform.rotation, target.rotation) > 0.01f)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, target.position, Time.deltaTime * Movespeed);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, target.rotation, Time.deltaTime * Movespeed);
            yield return null;
        }
        moving = false;
    }

    IEnumerator Teleport(Transform target)
    {
        moving = true;
        bool paused = true;
        yield return StartCoroutine(Fc.FadeIn(fadeDuration, () =>
        {
            paused = false;
        }));

        yield return new WaitUntil(() => !paused);

        player.transform.position = target.position;
        player.transform.rotation = target.rotation;
        yield return StartCoroutine(Fc.FadeOut(fadeDuration));
        moving = false;
    }

    void MoveBack()
    {
        if ((currentPos.x - 1) >= 0)
        {
            move(new Vector2Int(currentPos.x - 1, -1), TeleportType.Fade);
        }
        else if ((currentPos.x - 1) <= 0 || (currentPos.x - 1) > pos.Count)
        {
            move(new Vector2Int(0, -1), TeleportType.Fade);
        }
    }
}
