using UnityEngine;

public class SupportDriveScript : MonoBehaviour
{
    Rigidbody rb;
    float lastTimeChecked;

    public float antiRoll = 5000.0f;

    public WheelCollider[] frontWheels = new WheelCollider[2];
    public WheelCollider[] backWheels = new WheelCollider[2];


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.up.y > 0.5f || rb.linearVelocity.magnitude > 1f)
        {
            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked + 3f)
        {
            TurnBackCar();
        }
    }

    void TurnBackCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }

    void HoldWheelOnGround(WheelCollider[] wheels)
    {
        WheelHit hit;
        float leftRiding = 1.0f;
        float rightRiding = 1.0f;

        bool groundedL = wheels[0].GetGroundHit(out hit);
        if (groundedL)
        {
            leftRiding = (
                -wheels[0].transform.InverseTransformPoint(hit.point).y
                - wheels[0].radius
            ) / wheels[0].suspensionDistance;
        }
        else
        {
            leftRiding = 1.0f;
        }

        bool groundedR = wheels[1].GetGroundHit(out hit);
        if (groundedR)
        {
            rightRiding = (
                -wheels[1].transform.InverseTransformPoint(hit.point).y
                - wheels[1].radius
            ) / wheels[1].suspensionDistance;
        }
        else
        {
            rightRiding = 1.0f;
        }

        float antiRollForce = (leftRiding - rightRiding) * antiRoll;

        if (groundedL)
        {
            rb.AddForceAtPosition(wheels[0].transform.up * (-antiRollForce), wheels[0].transform.position);
        }

        if (groundedR)
        {
            rb.AddForceAtPosition(wheels[1].transform.up * (antiRollForce), wheels[1].transform.position);
        }
    }

    void FixedUpdate()
    {
        HoldWheelOnGround(frontWheels);
        HoldWheelOnGround(backWheels);
    }
}