using System.ComponentModel;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public int timer = 3;
    public static int totalLaps = 1;
    public static bool isRacing = false;
    public CheckpointController[] carsController;

    public TMP_Text startText;
    AudioSource audioSource;
    public AudioClip count;
    public AudioClip start;

    void CountDown()
    {
        startText.gameObject.SetActive(true);

        if (timer < 0)
        {
            //Debug.Log("Rozpoczêcie wyœcigu za: " + timer);
            startText.text = timer.ToString();
            audioSource.PlayOneShot(count);

            timer--;
        }
        else
        {
            //Debug.Log("Start!");
            startText.text = "START!!!";
            audioSource.PlayOneShot(start);

            isRacing = true;
            CancelInvoke("CountDown");
            Invoke("HideStartText", 1);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startText.gameObject.SetActive(false);

        InvokeRepeating("CountDown", 3, 1);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        carsController = new CheckpointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsController[i] = cars[i].GetComponent<CheckpointController>();
        }
    }

    void HideStartText()
    {
        startText.gameObject.SetActive(false);
    }
}
