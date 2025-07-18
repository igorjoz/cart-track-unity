using Unity.Cinemachine; // upewnij si�, �e masz t� using
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Przesuni�cia kamery (np. front, top-down itd.)")]
    public Vector3[] positions;

    [Header("Twoja Cinemachine Virtual Camera")]
    public CinemachineVirtualCamera cam;  // przywr�� ten typ

    int activePosition = 0;

    void Start()
    {
        if (positions.Length > 0)
        {
            var t = cam.GetCinemachineComponent<CinemachineTransposer>();
            t.m_FollowOffset = positions[0];
        }
    }

    void Update()
    {
        if (positions.Length > 0 && Input.GetKeyDown(KeyCode.T))
        {
            activePosition = (activePosition + 1) % positions.Length;
            var t = cam.GetCinemachineComponent<CinemachineTransposer>();
            t.m_FollowOffset = positions[activePosition];
        }
    }

    public void SetCameraProperties(GameObject car)
    {
        var drive = car.GetComponent<DrivingScript>();
        if (drive != null)
        {
            cam.Follow = drive.rb.transform;
            cam.LookAt = drive.cameraTarget;
        }
    }
}
