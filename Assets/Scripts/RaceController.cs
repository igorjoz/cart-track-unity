using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public int timer = 8;
    public static int totalLaps = 2;
    public static bool isRacing = false;

    public TMP_Text startText;
    AudioSource audioSource;
    public AudioClip countSound;
    public AudioClip startSound;

    public CheckpointController[] carsControllers;

    void CountDown()
    {
        Debug.Log("Rozpoczynam odlicanie");
        startText.gameObject.SetActive(true);

        if (timer > 0)
        {
            startText.text = timer.ToString();
            audioSource.PlayOneShot(countSound);

            Debug.Log("Rozpoczêcie wyœcigu za: " + timer);
            timer--;
        }
        else
        {
            startText.text = "START!";
            audioSource.PlayOneShot(startSound);

            Debug.Log("Start!");
            isRacing = true;
            CancelInvoke("CountDown");
            Invoke("HideStartText", 1);
        }
    }

    void HideStartText()
    {
        startText.gameObject.SetActive(false);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startText.gameObject.SetActive(false);

        InvokeRepeating("CountDown", 3, 1);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        carsControllers = new CheckpointController[cars.Length];

        for (int i = 0; i < cars.Length; i++)
        {
            carsControllers[i] = cars[i].GetComponent<CheckpointController>();
        }
    }

    private void LateUpdate()
    {
        int finishedLap = 0;

        foreach(CheckpointController controller in carsControllers)
        {
            if (controller.lap == totalLaps + 1)
            {
                finishedLap++;
            }

            if (finishedLap == carsControllers.Length && isRacing)
            {
                Debug.Log("Wyœcig skoñczony");
                isRacing = false;
            }
        }
    }
}
