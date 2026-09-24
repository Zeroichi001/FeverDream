using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class LobbyHandler : MonoBehaviour
{
    #region /// Global Variables ///

    //Refs
    [SerializeField] buttonHandler Play;
    [SerializeField] buttonHandler Continue;
    [SerializeField] buttonHandler NewGame;
    [SerializeField] buttonHandler Settings;
    [SerializeField] buttonHandler Credit;
    [SerializeField] buttonHandler Exit;
    [SerializeField] buttonHandler BackButton;
    [SerializeField] Transform MainPos;
    [SerializeField] Transform SettingPos;
    [SerializeField] Transform NewGamePos;
    [SerializeField] Transform CreditPos;

    GameManager GM => GameManager.instance;
    PlayerMain player => PlayerMain.instance;
    public static LobbyHandler instance;
    [SerializeField] GameObject PlayMenuParrent;

    //Vars
    [SerializeField] float UIduration = 1f;
    [SerializeField] float MoveDuration = 1f;
    [SerializeField] bool isNoSaved;
    enum lobbyState
    {
        Main,
        Play,
        NewGame,
        Setting,
        Credit
    }lobbyState state = lobbyState.Main;

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

    private void Controller()
    {
        if (Play.Key) state = lobbyState.Play;
        if (Settings.Key) state = lobbyState.Setting;
        if (Credit.Key) state = lobbyState.Credit;
        if (Exit.Key) state = lobbyState.Main;
        if (Input.GetKeyDown(KeyCode.Escape) && state != lobbyState.Main) state = lobbyState.Main;

        switch (state)
        {
            case lobbyState.Main :
                MainMenu();
                break;
            case lobbyState.Play :
                if (PlayUI == null) PlayUI = StartCoroutine(PlayUi());
                break;
            case lobbyState.NewGame :
                NewGameMenu();
                break;
            case lobbyState.Setting :
                SettingsMenu();
                break;
            case lobbyState.Credit :
                CreditMenu();
                break;
        }
        ButtonHandlers();
    }

    private void OnEnable() // Force teleport to main Menu //
    {
        if (!isStart) StartCoroutine(transition(false));
        state = lobbyState.Main;
    }

    private void OnDisable()
    {
        Play.Active = false; // will be erased in future
    }

    #endregion

    #region /// Methods ///

    private void MainMenu()
    {
        if (math.distance(MainPos.position, player.transform.position) > 0.01f || Quaternion.Angle(MainPos.rotation, player.transform.rotation) > 0.01f)
        {
            PlayerMain.instance.transform.SetPositionAndRotation(
                Vector3.Lerp(player.transform.position, MainPos.transform.position, Time.deltaTime * MoveDuration), 
                Quaternion.Lerp(player.transform.rotation, MainPos.transform.rotation, Time.deltaTime * MoveDuration)
                );
        }
    }

    Coroutine PlayUI;
    IEnumerator PlayUi()
    {
        float detract = -50f;
        float retract = 430f;

        RectTransform r = PlayMenuParrent.GetComponent<RectTransform>();

        PlayMenuParrent.SetActive(true);
        if (isNoSaved)
        {
            Continue.gameObject.SetActive(false);
        }
        else Continue.gameObject.SetActive(true);

        Play.Muted = true;
        Settings.Muted = true;
        Credit.Muted = true;
        while (math.distance(r.anchoredPosition.x, retract) > 1f)
        {
            Vector2 anchoredPos = r.anchoredPosition;
            anchoredPos.x = math.lerp(r.anchoredPosition.x, retract, Time.deltaTime * UIduration);
            Continue.Alpha = math.lerp(Continue.Alpha, 1f, Time.deltaTime * UIduration);
            NewGame.Alpha = math.lerp(NewGame.Alpha, 1f, Time.deltaTime * UIduration);

            Play.Alpha = math.lerp(Play.Alpha, .1f, Time.deltaTime * UIduration);
            Settings.Alpha = math.lerp(Settings.Alpha, .1f, Time.deltaTime * UIduration);
            Credit.Alpha = math.lerp(Credit.Alpha, .1f, Time.deltaTime * UIduration);

            r.anchoredPosition = anchoredPos;
            yield return null;
        }
        Vector2 retractSnap = new Vector2(retract, r.anchoredPosition.y);
        r.anchoredPosition = retractSnap;
        Continue.Alpha = 1f;
        NewGame.Alpha = 1f;
        Continue.Muted = false;
        NewGame.Muted = false;
        
        Continue.Muted = false;
        NewGame.Muted = false;

        while (true)
        {
            if (Continue.Key) Debug.Log("Continue");
            if (NewGame.Key)
            {
                state = lobbyState.NewGame;
                break;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || BackButton.Key)
            {
                state = lobbyState.Main;
                break;
            }
            yield return null;
        } // Main Code //

        Play.Muted = false;
        Settings.Muted = false;
        Credit.Muted = false;
        while (math.distance(r.anchoredPosition.x, detract) > 0.1f)
        {
            Vector2 anchoredPos = r.anchoredPosition;
            anchoredPos.x = math.lerp(r.anchoredPosition.x, detract, Time.deltaTime * UIduration);
            Continue.Alpha = math.lerp(Continue.Alpha, 0f, Time.deltaTime * UIduration);
            NewGame.Alpha = math.lerp(NewGame.Alpha, 0f, Time.deltaTime * UIduration);

            Play.Alpha = math.lerp(Play.Alpha, 1f, Time.deltaTime * UIduration);
            Settings.Alpha = math.lerp(Settings.Alpha, 1f, Time.deltaTime * UIduration);
            Credit.Alpha = math.lerp(Credit.Alpha, 1f, Time.deltaTime * UIduration);

            r.anchoredPosition = anchoredPos;
            yield return null;
        }
        Vector2 detractSnap = new Vector2(detract, r.anchoredPosition.y);
        r.anchoredPosition = detractSnap;
        Continue.Alpha = 0f;
        NewGame.Alpha = 0f;
        Play.Alpha = 1f;
        Settings.Alpha = 1f;
        Credit.Alpha = 1f;
        Continue.Muted = true;
        NewGame.Muted = true;

        PlayMenuParrent.SetActive(false);
        PlayUI = null;

        yield return null;
    }

    private void NewGameMenu()
    {
        if (math.distance(NewGamePos.position, player.transform.position) > 0.01f || Quaternion.Angle(NewGamePos.rotation, player.transform.rotation) > 0.01f)
        {
            PlayerMain.instance.transform.SetPositionAndRotation(
                Vector3.Lerp(player.transform.position, NewGamePos.transform.position, Time.deltaTime * MoveDuration),
                Quaternion.Lerp(player.transform.rotation, NewGamePos.transform.rotation, Time.deltaTime * MoveDuration)
                );
        }
    }

    private void SettingsMenu()
    {
        if (math.distance(SettingPos.position, player.transform.position) > 0.01f || Quaternion.Angle(SettingPos.rotation, player.transform.rotation) > 0.01f)
        {
            PlayerMain.instance.transform.SetPositionAndRotation(
                Vector3.Lerp(player.transform.position, SettingPos.transform.position, Time.deltaTime * MoveDuration),
                Quaternion.Lerp(player.transform.rotation, SettingPos.transform.rotation, Time.deltaTime * MoveDuration)
                );
        }
    }

    private void CreditMenu()
    {
        if (math.distance(CreditPos.position, player.transform.position) > 0.01f || Quaternion.Angle(CreditPos.rotation, player.transform.rotation) > 0.01f)
        {
            PlayerMain.instance.transform.SetPositionAndRotation(
                Vector3.Lerp(player.transform.position, CreditPos.transform.position, Time.deltaTime * MoveDuration),
                Quaternion.Lerp(player.transform.rotation, CreditPos.transform.rotation, Time.deltaTime * MoveDuration)
                );
        }
    }

    private void ButtonHandlers()
    {
        switch (state)
        {
            case lobbyState.Main:
                Play.Active = true;
                Settings.Active = true;
                Credit.Active = true;
                Exit.Active = false;
                break;
            case lobbyState.NewGame:
                Play.Active = false;
                Settings.Active = false;
                Credit.Active = false;
                Exit.Active = true;
                break;
            case lobbyState.Setting:
                Play.Active = false;
                Settings.Active = false;
                Credit.Active = false;
                Exit.Active = true;
                break;
            case lobbyState.Credit:
                Play.Active = false;
                Settings.Active = false;
                Credit.Active = false;
                Exit.Active = true;
                break;
        }
    }

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
        state = lobbyState.Main;
        yield return StartCoroutine(player.canvas.FadeC.FadeOut(fadeDuration));
    }

    #endregion
}
