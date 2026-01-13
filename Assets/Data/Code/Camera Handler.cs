using System.Collections.Generic;
using UnityEngine;
using static Player;

public class CameraHandler : MonoBehaviour
{
    public List<Vector3> office;

    playermanajer pm => playermanajer.instance;
    Player player => Player.instance;
    int location;

    private void Start()
    {
        changecamera();
    }

    private void Update()
    {
        if (location != pm.currentPos.x)
        {
            location = pm.currentPos.x;
            changecamera();
        }
    }

    void changecamera()
    {
        switch(location)
        {
            case 0:
                player.SwitchMethod(cameramethod.swipe,
            new MethodParam
            {
                LookEuler = office,
                lookDefault = 1
            }
            );
                break;
            default:
                resetCamera();
                break;  
        }
    }

    void resetCamera()
    {
            
    }
}
