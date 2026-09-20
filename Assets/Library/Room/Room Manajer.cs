using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RoomManajer : MonoBehaviour
{
    #region GlobalVariables
    public static RoomManajer instance;

    [SerializeField] private RoomEntry[] rooms;
    public int RoomLength => rooms.Length;
    public RoomEntry.RoomID CurrentPos { get; private set; }

    //References
    PlayerMain player => PlayerMain.instance;
    [SerializeField] GameObject LobbyUIParent;

    //System
    public Vector2 Timer { get; private set; } = new Vector2(0, 0);
    public int Interval { get => 10 - Mathf.RoundToInt(Timer.x); }

    #endregion

    #region Core ///Core of script///
    private void Awake() // Initialize instance
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
    }

    private void Update() // still in development
    {
        Counter();
    }

    bool isLobbyUIActive = false;
    private void OnDisable() // Function Run in Disable, turn off every handlers //
    {
        foreach (var r in rooms)
        {
            if (!r.handler.isActiveAndEnabled) return;
            r.handler.enabled = false;
        }
        isLobbyUIActive = true;
    }

    private void OnEnable() // Function Run in Enable, teleporting to office when enabled //
    {
        Transform target = null;
        foreach (var r in rooms)
        {
            if (r.id == RoomEntry.RoomID.OfficeRoom)
            {
                target = r.SpawnPoint;
            }
        }

        RoomHandlerers(RoomEntry.RoomID.OfficeRoom, target);
        Timer = new Vector2(0, 0);
    }

    void Counter() // Time Counter // 
    {
        Timer = new Vector2(Timer.x, Timer.y + Time.deltaTime);
        if (Timer.y >= 60)
        {
            Timer = new Vector2(Timer.x + 1, 0);
        }
    }

    #endregion

    #region Methods ///Main Method use in this Script///

    Coroutine move;
    void RoomHandlerers(RoomEntry.RoomID target,Transform pos) // Move to target location //
    {
        if (move != null) return;

        move = StartCoroutine(Move(target, pos));
    }

    IEnumerator Move(RoomEntry.RoomID target, Transform pos) //Fade Move//
    {
        float fadeDuration = 0.5f;
        bool paused = true;
        yield return StartCoroutine(player.canvas.FadeC.FadeIn(fadeDuration, () =>
        {
            paused = false;
        }));
        yield return new WaitUntil(() => !paused);

        player.gameObject.transform.position = pos.position;
        player.gameObject.transform.rotation = pos.rotation;
        CurrentPos = target;

        if (isLobbyUIActive) LobbyUIParent.SetActive(false);
        
        foreach (var r in rooms)
        {
            if (r.id == target)
            {
                r.handler.enabled = true;
            }
            else
            {
                r.handler.enabled = false;
            }
        }

        yield return new WaitForSeconds(fadeDuration);

        yield return StartCoroutine(player.canvas.FadeC.FadeOut(fadeDuration));
        move = null;
    }
    IEnumerator AnimatedMove(int target) // Animated Move still in Development //
    {
        yield return null;
    }

    public RoomEntry GetRoom(RoomEntry.RoomID ID) //Room lookup//
    {
        foreach (var room in rooms)
        {
            if (room.id == ID)
            {
                return room;
            }
        }
        return null;
    }

    #endregion
}

[System.Serializable]
public class RoomEntry //Room Container//
{
    public RoomID id;
    public MonoBehaviour handler;
    public Transform SpawnPoint;
    public GameObject Parrent => handler.gameObject;
    public RoomID[] Neighbors;

    public enum RoomID
    {
        DiningRoom,
        StorageRoom,
        Kitchen,
        EmployeeRoom,
        OfficeRoom,
        Hallway,
    }
}
