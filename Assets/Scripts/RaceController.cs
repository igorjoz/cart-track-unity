using System.ComponentModel;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public int timer = 3;
    public static int totalLaps = 1;
    public static bool isRacing = false;
    public CheckpointController[] carsController;

    void CountDown()
    {
        if (timer < 0)
        {
            Debug.Log("Rozpoczêcie wyœcigu za: " + timer);
            timer--;
        }
        else
        {
            Debug.Log("Start!");
            isRacing = true;
            CancelInvoke("CountDown");
        }
    }

    void Start()
    {
        InvokeRepeating("CountDown", 3, 1);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        carsController = new CheckpointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsController[i] = cars[i].GetComponent<CheckpointController>();
        }

    }
}
