using Unity.Cinemachine; // upewnij si�, �e masz t� using
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Przesuni�cia kamery (np. front, top-down itd.)")]
    public Vector3[] positions;

    [Header("Twoja Cinemachine Virtual Camera")]
    public CinemachineCamera cam;  // zaktualizowany typ

    int activePosition = 0;

    void Start()
    {
        if (positions.Length > 0)
        {
            var follow = cam.GetComponent<CinemachineFollow>();
            if (follow != null)
                follow.FollowOffset = positions[0];
        }
    }

    void Update()
    {
        if (positions.Length > 0 && Input.GetKeyDown(KeyCode.T))
        {
            activePosition = (activePosition + 1) % positions.Length;
            var follow = cam.GetComponent<CinemachineFollow>();
            if (follow != null)
                follow.FollowOffset = positions[activePosition];
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
