using UnityEngine;
using Unity.Cinemachine;    // ← CinemachineFollow lives here

public class CameraController : MonoBehaviour
{
    public Vector3[] positions;
    public CinemachineCamera cam;

    int activePosition = 0;
    CinemachineFollow follow;

    void Awake()
    {
        if (cam == null)
        {
            Debug.LogError("Assign your CinemachineCamera in the Inspector!");
            return;
        }

        // non-generic lookup of the Body-stage component
        follow = cam.GetCinemachineComponent(CinemachineCore.Stage.Body)
                    as CinemachineFollow;
        if (follow == null)
            Debug.LogError("Position Control isn’t set to ‘Follow’ on your camera.");
    }

    void Start()
    {
        if (follow != null && positions.Length > 0)
            follow.FollowOffset = positions[activePosition];
    }

    void Update()
    {
        if (follow == null || positions.Length == 0) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            activePosition = (activePosition + 1) % positions.Length;
            follow.FollowOffset = positions[activePosition];
        }
    }

    public void SetCameraProperties(GameObject car)
    {
        var drive = car.GetComponent<DrivingScript>();
        if (drive == null) return;
        cam.Follow = drive.rb.transform;
        cam.LookAt = drive.cameraTarget;
    }
}
