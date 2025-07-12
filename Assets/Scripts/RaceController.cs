using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RaceController : MonoBehaviourPunCallbacks
{
    public CheckPointController[] carsController;
    public static bool racing = false;
    public static int totalLaps = 1;
    public int timer = 3;

    [SerializeField] private Text startText;
    [SerializeField] private AudioClip countClip;
    [SerializeField] private AudioClip startClip;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingText;
    [SerializeField] private RawImage mirror;

    private AudioSource audioSource;
    private int playerCount;

    void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        endPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        startText.gameObject.SetActive(false);
        startButton.SetActive(false);
        waitingText.SetActive(false);

        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        Vector3 pos = spawnPos[idx].position;
        Quaternion rot = spawnPos[idx].rotation;

        object[] instData = {
            PlayerPrefs.GetString("PlayerName"),
            PlayerPrefs.GetInt("Red"),
            PlayerPrefs.GetInt("Green"),
            PlayerPrefs.GetInt("Blue")
        };

        GameObject playerCar = PhotonNetwork.Instantiate(
            carPrefab.name, pos, rot, 0, instData
        );
        playerCar.GetComponent<CarAppearance>().SetLocalPlayer();

        if (PhotonNetwork.IsMasterClient)
            startButton.SetActive(true);
        else
            waitingText.SetActive(true);

        playerCar.GetComponent<DrivingScript>().enabled = true;
        playerCar.GetComponent<PlayerController>().enabled = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // opcjonalnie: aktualizuj UI, jeżeli potrzebujesz
    }

    void LateUpdate()
    {
        int finished = 0;
        foreach (var ctrl in carsController)
            if (ctrl.lap == totalLaps + 1) finished++;

        if (finished == carsController.Length && racing)
        {
            endPanel.SetActive(true);
            racing = false;
        }
    }

    void CountDown()
    {
        startText.gameObject.SetActive(true);
        if (timer > 0)
        {
            startText.text = timer.ToString();
            audioSource.PlayOneShot(countClip);
            timer--;
        }
        else
        {
            startText.text = "START!!!";
            audioSource.PlayOneShot(startClip);
            racing = true;
            CancelInvoke(nameof(CountDown));
            Invoke(nameof(HideStartText), 1f);
        }
    }

    void HideStartText() => startText.gameObject.SetActive(false);

    public void BeginGame()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(StartGame), RpcTarget.All);
    }

    [PunRPC]
    public void StartGame()
    {
        InvokeRepeating(nameof(CountDown), 3f, 1f);
        startButton.SetActive(false);
        waitingText.SetActive(false);

        var cars = GameObject.FindGameObjectsWithTag("Car");
        carsController = new CheckPointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
            carsController[i] = cars[i].GetComponent<CheckPointController>();
    }

    public void SetMirror(Camera backCam)
    {
        mirror.texture = backCam.targetTexture;
    }
}
