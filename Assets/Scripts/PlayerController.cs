using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;

    void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
    }

    void Update()
    {
        float accelerator = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");

        if (!RaceController.isRacing)
        {
            accelerator = 0;
        }

        drivingScript.Drive(accelerator, brake, steer);
    }
}
