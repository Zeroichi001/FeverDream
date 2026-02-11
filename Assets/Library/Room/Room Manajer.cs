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
    public RoomEntry.RoomID CurrentPos { get; private set; }
    public RoomEntry.RoomID targetPos;

    //References
    PlayerMain player => PlayerMain.instance;
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

    private void Start()
    {
        CurrentPos = targetPos;
        RoomHandlerers();
    }

    private void Update()
    {
        if (CurrentPos != targetPos)
        {
            CurrentPos = targetPos;
            RoomHandlerers();
        }
    }
    #endregion

    #region Methods ///Main Method use in this Script///
    void RoomHandlerers()
    {
        Transform target = null;
        foreach (var r in rooms)
        {
            if (r.id != CurrentPos)
            {
                r.handler.enabled = false;
            }
            else
            {
                target = r.spawnPoint;
                r.handler.enabled = true;
            }
        }
        StartCoroutine(Move(target));
    }

    IEnumerator Move(Transform target)
    {
        float fadeDuration = 0.5f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime / fadeDuration;
            player.canvas.fade = Mathf.Lerp(player.canvas.fade, 1f, timer);
            yield return null;
        }
        timer = 0f;
        player.gameObject.transform.position = target.position;
        player.gameObject.transform.rotation = target.rotation;
        yield return new WaitForSeconds(fadeDuration);

        while (timer < 1f)
        {
            timer = Time.deltaTime / fadeDuration;
            player.canvas.fade = Mathf.Lerp(player.canvas.fade, 0f, timer);
            yield return null;
        }
    }
    #endregion
}

[System.Serializable]
public class RoomEntry
{
    public RoomID id;
    public MonoBehaviour handler;
    public Transform spawnPoint;

    public enum RoomID
    {
        OfficeRoom,
        EmployeRoom
    }
}
