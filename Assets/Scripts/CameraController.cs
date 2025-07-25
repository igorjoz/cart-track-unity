using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public Vector3[] positions;
    public CinemachineCamera cam;
    public Transform followTarget;
    public Transform lookAtTarget;
    
    int activePosition = 0;
    
    void Start()
    {
        if (positions.Length == 0) return;
        UpdateCameraPosition();
    }
    
    void Update()
    {
        if (positions.Length == 0) return;
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            activePosition++;
            activePosition = activePosition % positions.Length;
            UpdateCameraPosition();
        }
    }
    
    void UpdateCameraPosition()
    {
        if (cam != null && positions.Length > 0)
        {
            // In Cinemachine 3.x, we access the CinemachineFollow component
            var followComponent = cam.GetComponent<CinemachineFollow>();
            if (followComponent != null)
            {
                followComponent.FollowOffset = positions[activePosition];
            }
        }
    }
    
    public void SetCameraProperties(GameObject car)
    {
        var drivingScript = car.GetComponent<DrivingScript>();
        if (drivingScript == null) 
        {
            Debug.LogError("DrivingScript component not found on car object!");
            return;
        }
        
        if (cam != null)
        {
            // Set follow target
            cam.Follow = drivingScript.rb.transform;
            
            // Set look at target
            if (drivingScript.cameraTarget != null)
            {
                cam.LookAt = drivingScript.cameraTarget.transform;
            }
            else
            {
                Debug.LogWarning("Camera target not set on DrivingScript!");
                cam.LookAt = drivingScript.rb.transform; // Use the car itself as fallback
            }
        }
    }
}
