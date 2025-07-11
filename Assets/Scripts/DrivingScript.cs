using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour
{
    public WheelScript[] wheels; 
    public float torque = 200; 
    public float maxSteerAngle = 30; 
    public float maxBrakeTorque = 500; 
    public float maxSpeed = 150; 
    public Rigidbody rb;

    public float currentSpeed;
    public Transform cameraTarget;

    public void Drive(float accel, float brake, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        float thrustTorque = 0;

        currentSpeed = rb.linearVelocity.magnitude * 5;
        if (currentSpeed < maxSpeed) thrustTorque = accel * torque;


        foreach (WheelScript wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = thrustTorque;
            if (wheel.frontWheel) wheel.wheelCollider.steerAngle = steer;
            else wheel.wheelCollider.brakeTorque = brake;


            Quaternion quat;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out quat);
            wheel.wheel.transform.position = position;
            wheel.wheel.transform.rotation = quat;
        }
    }
}
