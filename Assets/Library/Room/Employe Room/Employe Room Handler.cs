using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeRoomHandler : MonoBehaviour
{
    #region GlobalVariables

    public EmployeCamera cam;

    #endregion

    #region Core

    private void OnEnable()
    {
        cam.enabled = true;
    }
    private void OnDisable()
    {
        cam.enabled = false;
    }
    #endregion
}
