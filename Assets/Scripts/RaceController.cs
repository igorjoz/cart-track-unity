using System.ComponentModel;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public int timer = 3;
    public static int totalLaps = 1;
    public static bool isRacing = false;

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
    }
}
