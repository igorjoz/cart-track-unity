using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;
    float lastTimeFromMove = 0;
    CheckpointController checkpointController;

    void RestartLayer()
    {
        drivingScript.gameObject.layer = 0;
    }

    void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
        checkpointController = drivingScript.rb.GetComponent<CheckpointController>();
    }

    void Update()
    {
        float accelerator = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");

        if (drivingScript.rb.linearVelocity.magnitude > 1 || !RaceController.isRacing)
        {
            lastTimeFromMove = Time.time;
        }

        if (Time.time > lastTimeFromMove + 4 || drivingScript.rb.gameObject.transform.position.y < -5)
        {
            drivingScript.rb.transform.position = checkpointController.lastCheckpoint.transform.position + Vector3.up * 2;

            // drivingScript.rb.transform.rotation = checkpointController.lastCheckpoint.transform.rotation;

            drivingScript.gameObject.layer = 6;

            Invoke("ResetLayer", 3);
        }

        if (!RaceController.isRacing)
        {
            accelerator = 0;
        }

        drivingScript.Drive(accelerator, brake, steer);
    }
}
