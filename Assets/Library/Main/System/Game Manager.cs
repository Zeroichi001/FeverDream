using System.Diagnostics.Contracts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Main system run here to manage all game systems
    // UI handler, Audio handler, Game state handler, etc.

    #region GlobalVariables

    // References
    RoomManajer roomManager => RoomManajer.instance;
    LobbyHandler lobbyHandler => LobbyHandler.instance;
    PlayerMain player => PlayerMain.instance;
    public static GameManager instance;

    [SerializeField] buttonHandler exit;

    //Variables
    public int Difficulty { get; private set; } = 0;

    #endregion

    #region MainSystem
    
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
    }

    private void Start()
    {
        lobbyHandler.enabled = true;
    }

    void Update()
    {
        if (exit.Key)
        {
            MainMenu();
        }
    }

    #endregion

    #region Methods
    public void PlayScene()
    {
        roomManager.enabled = true;
        lobbyHandler.enabled = false;
    }

    public void MainMenu()
    {
        roomManager.enabled = false;
        lobbyHandler.enabled = true;
    }

    public void difficulty(int value)
    {
        Difficulty = value;
    }

    #endregion
}
