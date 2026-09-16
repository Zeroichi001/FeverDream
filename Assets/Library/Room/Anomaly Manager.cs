using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; }

    public AnomalyData anomalyData = new AnomalyData();
    public Dictionary<string, int> CurrentNight = new Dictionary<string, int>();
    public Dictionary<string, int> ActiveAnomaly = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        anomalyData.awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("AnomalyManager: F1 key pressed");
            foreach (var room in anomalyData.anomaly)
            {
                foreach (var anomaly in room.anomalies)
                {
                    Debug.Log($"Room: {room.room}, Anomalous: {anomaly.Anomalous.name}, Normal: {anomaly.Normal.name}");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int RNG = Random.Range(0, 20);
            int RoomRNG = Random.Range(0, anomalyData.anomaly.Length);

            Debug.Log("Decision " + RNG + " room " + anomalyData.getRoom(RoomRNG).room);
        }
    }

    private void OnStartup()
    {
        CurrentNight.Clear();
        ActiveAnomaly.Clear();

        for (int i = 0; i < anomalyData.anomaly.Length; i++)
        {
            for (int j = 0; j < anomalyData.anomaly[i].anomalies.Length; j++)
            {
                CurrentNight.Add(anomalyData.anomaly[i].anomalies[j].Name, anomalyData.anomaly[i].anomalies[j].Weight);
            }
        }
    }

    private void RNGanomaly()
    {
        int value = 20;
        List<string> ID = new List<string>();

        foreach (var anomaly in CurrentNight)
        {
            if (anomaly.Value <= value)
            {
                ID.Add(anomaly.Key);
            }
        }

        int Difficulty = 5;
        if (Random.Range(1 , 20) <= Difficulty)
        {
            int RNG = Random.Range(0, ID.Count);
            for (int i = 0; i < ID.Count; i++)
            {
                if (ID[i] == ID[RNG])
                {
                    ActiveAnomaly.Add(ID[i], CurrentNight[ID[i]]);
                    CurrentNight.Remove(ID[i]);
                    anomalyData.ActiveAnomaly(ID[i]);
                }
            }
        }
    }///Random Anomaly Activator
}

[System.Serializable]
public class AnomalyData
{
    public Data[] anomaly;

    [System.Serializable]
    public class Data
    {
        public RoomEntry.RoomID room;
        public Anomaly[] anomalies;

        [System.Serializable]
        public class Anomaly
        {
            public string Name;
            public int Level;
            public int Weight;
            public GameObject Normal;
            public GameObject Anomalous;
            public bool isSingle;
        }
    }

    public void awake()
    {
        foreach (var room in anomaly)
        {
            foreach (var anomaly in room.anomalies)
            {
                if (anomaly.Anomalous.GetComponent<BoxCollider>() == null)
                {
                    anomaly.Anomalous.AddComponent<BoxCollider>();
                    Debug.LogWarning("BoxCollider added to Anomalous GameObject: " + anomaly.Anomalous.name);
                }
            }
        }
    }

    public Data getRoom(int i)
    {
        return anomaly[i];
    }

    public void ClearAnomaly(RoomEntry.RoomID roomID, GameObject _anomaly)
    {
        Data current = null;

        for (int i = 0; i < anomaly.Length; i++)
        {
            if (anomaly[i].room == roomID)
            {
                current = anomaly[i];
            }
        }

        for (int i = 0; i < current.anomalies.Length; i++)
        {
            if (current.anomalies[i].Anomalous == _anomaly)
            {
                if (current.anomalies[i].isSingle)
                {
                    current.anomalies[i].Anomalous.SetActive(false);
                }
                else
                {
                    current.anomalies[i].Normal.SetActive(true);
                    current.anomalies[i].Anomalous.SetActive(false);
                }
            }
        }
    }// clear anomaly

    public void ActiveAnomaly(string ID)
    {
        for (int i = 0; i < anomaly.Length; i++)
        {
            for (int j = 0; j < anomaly[i].anomalies.Length; j++)
            {
                if (anomaly[i].anomalies[j].Name == ID)
                {
                    if (anomaly[i].anomalies[j].isSingle)
                    {
                        anomaly[i].anomalies[j].Anomalous.SetActive(true);
                    }
                    else
                    {
                        anomaly[i].anomalies[j].Normal.SetActive(false);
                        anomaly[i].anomalies[j].Anomalous.SetActive(true);
                    }
                }
            }
        }
    }// add anomaly
}
