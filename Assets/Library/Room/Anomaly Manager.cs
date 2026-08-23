using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; }

    public AnomalyData anomalyData = new AnomalyData();

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
                current.anomalies[i].Normal.SetActive(true);
                current.anomalies[i].Anomalous.SetActive(false);
            }
        }
    }// clear anomaly
}
