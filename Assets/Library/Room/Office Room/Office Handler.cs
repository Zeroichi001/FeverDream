using UnityEngine;

public class OfficeHandler : MonoBehaviour
{
    [SerializeField] OfficeState CurrentState;

    [SerializeField] MonoBehaviour localHandler, CamHandler, DoorHandler;

    #region Core ///Core of script///
    [System.Serializable]
    public class curve
    {
        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Z;
    }

    public enum OfficeState
    {
        Local,
        Cam,
        Door
    }

    private void Update()
    {
        
    }
    #endregion

    #region methods ///Main Method use in this Script///
    public void RequestSwitchState(OfficeState target)
    {
        if (CurrentState != OfficeState.Local) return;

        localHandler.enabled = false;
        switch (target)
        {
            case OfficeState.Cam:
                CamHandler.enabled = true;
                DoorHandler.enabled = false;
                break;
            case OfficeState.Door:
                CamHandler.enabled = false;
                DoorHandler.enabled = true;
                break;
        }
        CurrentState = target;
    }
    #endregion  
}
