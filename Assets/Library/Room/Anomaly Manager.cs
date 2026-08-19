using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnomalyManager : MonoBehaviour
{
    [System.Serializable]
    public class AnomalyData
    {
        public Data[] anomaly;
        public class Data
        {
            public RoomList room;
            public Anomaly[] anomalies;

            public class Anomaly
            {
                public GameObject Normal;
                public GameObject Anomalous;
                public bool isSingle;
            }
        }

        public enum RoomList
        {
            Dining,
            kitchen,
            Storage,
            Employee
        }
    }
    public AnomalyData anomalyData = new();
}
