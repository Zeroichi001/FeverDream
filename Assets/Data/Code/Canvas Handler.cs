using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    playermanajer pm => playermanajer.instance;
    Player plyr => Player.instance;

    [SerializeField] private Image buttonBottom;
    [SerializeField] private Image buttonleft;
    [SerializeField] private Image buttonRight;

    int last;

    private void Update()
    {
        if (pm.moving || pm.currentPos.x == 0)
        {
            buttonBottom.enabled = false;
        } else buttonBottom.enabled = true;

        if (plyr.currentCamMethod == Player.cameramethod.swipe)
        {
            swipeCam();
        }else
        {
            last = -1;
            buttonleft.enabled = false;
            buttonRight.enabled = false;
        }
    }

    void swipeCam()
    {
        if (last == plyr.sharedParam.state.currentRot) return;
        
        last = plyr.sharedParam.state.currentRot;

        if (plyr.sharedParam.state.currentRot == plyr.sharedParam.var.LookEuler.Count -1)
        {
            buttonleft.enabled = true;
            buttonRight.enabled = false;
        }else if (plyr.sharedParam.state.currentRot == 0)
        {
            buttonleft.enabled = false;
            buttonRight.enabled = true;
        }
        else
        {
            buttonleft.enabled = true;
            buttonRight.enabled = true;
        }
    }
}
