using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        float accelerator = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");

        drivingScript.Drive(accelerator, steer, brake);
    }
}
