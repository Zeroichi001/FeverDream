using System.Collections.Generic;
using UnityEngine;
using static Player;

public class CameraHandler : MonoBehaviour
{
    public List<Vector3> office;

    playermanajer pm => playermanajer.instance;
    Player player => Player.instance;
    int location;
    bool forOffice;

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

        if (pm.currentPos.x == 1)
        {
            forOffice = true;
        }else forOffice = false;
    }

    void changecamera()
    {
        int lastState;
        if (forOffice)
        {
            lastState = 2;
        } else
        {
            lastState = -1;
        }
            switch (location)
            {
                case 0:
                    player.SwitchMethod(cameramethod.swipe,
                new MethodParam
                {
                    LookEuler = office,
                    lookDefault = 1,
                    lastLook = lastState
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
