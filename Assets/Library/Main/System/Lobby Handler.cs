using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LobbyHandler : MonoBehaviour
{
    #region /// Global Variables ///

    //Refs
    [SerializeField] buttonHandler Play;
    //[SerializeField] buttonHandler Exit;
    [SerializeField] Transform MainPos;
    [SerializeField] Transform SettingPos;
    [SerializeField] Transform PlayPos;
    [SerializeField] Transform CreditPos;

    GameManager GM => GameManager.instance;
    PlayerMain player => PlayerMain.instance;
    public static LobbyHandler instance;

    //Vars
    [SerializeField] float duration = 1f;

    #endregion

    #region /// Mains ///

    private void Awake()
    {
        instance = this;
    }

    bool isStart = true;
    private void Start()
    {
        StartCoroutine(transition(true));
        isStart = false;
    }

    private void Update()
    {
        Controller();
    }

    enum LobbyState
    {
        Main,
        Setting,
        Play,
        Credit
    }
    LobbyState currentState;
    IEnumerator PosChange(LobbyState target) // Move To Target Pos // 
    {
        if (currentState == target) yield break;

        float t = 0f;
        Transform targetPos = null;
        switch(target)
        {
            case LobbyState.Main:
                targetPos = MainPos;
                break;
            case LobbyState.Setting:
                targetPos = SettingPos;
                break;
            case LobbyState.Play:
                targetPos = PlayPos;
                break;
            case LobbyState.Credit:
                targetPos = CreditPos;
                break;
        }
        
        Vector3 Pos = PlayerMain.instance.transform.position;
        Quaternion Rot = PlayerMain.instance.transform.rotation;

        while ((duration - t) > 0.01f)
        {
            t += Time.deltaTime;

            PlayerMain.instance.transform.SetPositionAndRotation(Vector3.Lerp(Pos, targetPos.position, t / duration), Quaternion.Lerp(Rot, targetPos.rotation, t / duration));
        }
        PlayerMain.instance.transform.SetPositionAndRotation(targetPos.position, targetPos.rotation);

        yield return null;
    }

    private void Controller()
    {
        if (Play.Key)
        {
            GM.PlayScene();
        }
    }

    private void OnEnable() // Force teleport to main Menu //
    {
        if (!isStart) StartCoroutine(transition(false));
        currentState = LobbyState.Main;
    }

    private void OnDisable()
    {
        Play.Active = false;
    }

    #endregion

    #region /// Methods ///

    IEnumerator transition(bool start) //Fade transition//
    {
        float fadeDuration = 0.5f;
        bool paused = true;
        
        if (!start)
        {
            yield return StartCoroutine(player.canvas.FadeC.FadeIn(fadeDuration, () =>
            {
                paused = false;
            }));

            yield return new WaitUntil(() => !paused);
        }
        else
        {
            player.canvas.FadeC.Alpha = 1f;
        }

        //foreach (var g in SceneUIParrent)
        //{
        //    if (g.gameObject.activeInHierarchy) g.gameObject.SetActive(false);
        //}

        player.gameObject.transform.position = MainPos.position;
        player.gameObject.transform.rotation = MainPos.rotation;
        Play.Active = true;

        //foreach (var r in rooms)
        //{
        //    if (r.id == target)
        //    {
        //        r.handler.enabled = true;
        //    }
        //    else
        //    {
        //        r.handler.enabled = false;
        //    }
        //}

        yield return new WaitForSeconds(fadeDuration);

        yield return StartCoroutine(player.canvas.FadeC.FadeOut(fadeDuration));
    }

    #endregion
}
