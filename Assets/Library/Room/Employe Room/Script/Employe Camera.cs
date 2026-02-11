using Unity.VisualScripting;
using UnityEngine;

public class EmployeCamera: MonoBehaviour
{
    public float maxYaw;
    public float maxPitch;
    public float cameraSpeed = 5f;

    PlayerMain player => PlayerMain.instance;
    Camera cam => player.cam;

    bool flashSwitch;
    private void Update()
    {
        Camera();
        flashLight();
    }

    void Camera()
    {
        // Ambil posisi kursor relatif terhadap lebar & tinggi layar (0 - 1)
        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;

        // Konversi ke -1 sampai 1 (tengah = 0)
        float normalizedX = Mathf.Clamp((mouseX - 0.5f) * 2f, -1f, 1f);
        float normalizedY = Mathf.Clamp((mouseY - 0.5f) * 2f, -1f, 1f);

        // Tentukan target rotasi berdasarkan posisi kursor
        float targetYaw = normalizedX * maxYaw;
        float targetPitch = -normalizedY * maxPitch;

        // Smooth rotasi (biar gak patah-patah)
        Quaternion target = Quaternion.Euler(targetPitch, targetYaw, 0f);
        cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, target, Time.deltaTime * cameraSpeed);
    }

    void flashLight()
    {
        if (player.flashLight.activeInHierarchy) player.flashLight.SetActive(true);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.origin + ray.direction * 10f;
        player.flashLight.transform.LookAt(targetPoint);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            flashSwitch = !flashSwitch;
            if (flashSwitch)
            {
                player.flashLight.transform.GetChild(0).gameObject.SetActive(true);
            }
            else player.flashLight.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
