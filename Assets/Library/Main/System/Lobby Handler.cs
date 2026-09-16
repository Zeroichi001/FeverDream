using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    [SerializeField] buttonHandler Play;

    RoomManajer RM => RoomManajer.instance;

    private void Update()
    {
        if (Play.Key)
        {
            PlayScene();
        }
    }

    private void PlayScene()
    {
        RM.GetComponent<RoomManajer>().enabled = true;
        Play.gameObject.SetActive(false);
    }
}
