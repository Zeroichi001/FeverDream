using System.Collections.Generic;
using UnityEngine;
using static GameRefs;

[CreateAssetMenu(fileName = "GameSetting", menuName = "Scriptable Objects/GameSetting")]
public class initializeGameSetting : ScriptableObject
{
    #region Refs

    public AudioLib audioLib = new AudioLib();

    #endregion

    private void Awake()
    {
        audioLib.initialize();
    }
}

public static class GameSetting
{
    [Range(0, 100)] public static int MainVolume = 100;
}

public static class GameRefs
{
    public static Dictionary<string, AudioClip> AudioLib;
}

[System.Serializable] public class AudioLib
{
    [SerializeField] private Audio[] audioList;

    [System.Serializable] public struct Audio
    {
        public string name;
        public AudioClip clip;
    }

    public void initialize()
    {
        if (GameRefs.AudioLib != null) GameRefs.AudioLib.Clear();

        foreach (var audio in audioList)
        {
            GameRefs.AudioLib.Add(audio.name, audio.clip);
        }
    }
}