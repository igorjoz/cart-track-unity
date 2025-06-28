using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;
    float lastTimeMoving = 0;
    CheckpointController checkPointController;

    void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
        checkPointController = drivingScript.rb.GetComponent<CheckpointController>();
    }

    void Update()
    {
        float accelerator = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");

        if (drivingScript.rb.linearVelocity.magnitude > 1 || !RaceController.isRacing)
        {
            lastTimeMoving = Time.time;
        }

        if (Time.time > lastTimeMoving + 4 || drivingScript.rb.gameObject.transform.position.y < -5)
        {
            drivingScript.rb.transform.position =
checkPointController.lastCheckpoint.transform.position + Vector3.up * 2;
            drivingScript.rb.transform.rotation =
            checkPointController.lastCheckpoint.transform.rotation;
            drivingScript.rb.gameObject.layer = 6;
            Invoke("ResetLayer", 3);
        }


        if (!RaceController.isRacing)
        {
            accelerator = 0;
        }

        drivingScript.Drive(accelerator, brake, steer);
    }

    void ResetLayer()
    {
        drivingScript.rb.gameObject.layer = 0;
    }
}
