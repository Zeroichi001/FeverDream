using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    public Vector2Int targetPos;
    public bool enableHover;

    playermanajer pm => playermanajer.instance;
    CameraMonitor screen;
    Player player;

    void Awake()
    {
        screen = gameObject.GetComponentInChildren<CameraMonitor>();
        player = Camera.main.GetComponent<Player>();
    }
    private void Update()
    {
        if (pm.currentPos == targetPos)
        {
            screen.isFocused = true;
            enableHover = false;
            //player.activeCam = false;
        } else
        {
            screen.isFocused = false;
            enableHover = true;
            //player.activeCam = true;
        }

        if (enableHover)
        {
            hover();
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    private void hover()
    {
        if (pm.hit.collider != null)
        {
            if (pm.hit.collider.gameObject == gameObject)
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }
            else gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    public void select()
    {
        pm.move(targetPos, playermanajer.TeleportType.Walk);
    }
}