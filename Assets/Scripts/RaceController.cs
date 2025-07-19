// RaceController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RaceController : MonoBehaviourPunCallbacks
{
    [Header("Race Settings")]
    public int countdownStart = 3;
    public static int totalLaps = 2;
    public static bool isRacing = false;

    [Header("UI Elements")]
    public GameObject endRacePanel;
    public TMP_Text startText;
    public Button startButton;
    public GameObject waitingText;

    [Header("Car Prefab & Spawns")]
    public GameObject carPrefab;      // musi leżeć w Resources/
    public Transform[] spawnPositions;

    [Header("Audio Clips")]
    public AudioClip countSound;
    public AudioClip startSound;

    private AudioSource audioSource;
    private int timer;
    private CheckpointController[] carsControllers;

    void Awake()
    {
        // Wstępna konfiguracja UI i audio
        audioSource = GetComponent<AudioSource>();
        endRacePanel.SetActive(false);
        startText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        waitingText.SetActive(false);

        // Synchronizuj scenę automatycznie przez Photona
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InRoom)
        {
            SpawnLocalCar();
            SetupStartUI();
        }
    }

    void Start()
    {
        timer = countdownStart;

        // Jeśli klient jest już w pokoju (np. szybkie dołączenie),
        // od razu spawn i konfiguruj UI
        //if (PhotonNetwork.InRoom)
        //{
        //    SpawnLocalCar();
        //    SetupStartUI();
        //}
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // Gdy callback potwierdzi dołączenie: stwórz auto i przygotuj UI
        SpawnLocalCar();
        SetupStartUI();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        // Jeśli to MY zostałem nowym hostem, przypnij kamerę do mojego auta
        if (PhotonNetwork.IsMasterClient)
        {
            var camCtrl = FindFirstObjectByType<CameraController>();
            if (camCtrl != null)
            {
                // Znajdź moje auto i przypnij do niego kamerę
                var myCar = FindMyLocalCar();
                if (myCar != null)
                {
                    camCtrl.SetCameraProperties(myCar);
                }
            }
        }
    }

    /// <summary>
    /// Znajduje auto lokalnego gracza (które należy do tego klienta)
    /// </summary>
    private GameObject FindMyLocalCar()
    {
        var cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (var car in cars)
        {
            var photonView = car.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                return car;
            }
        }
        return null;
    }

    /// <summary>
    /// Wyświetla MasterClientowi przycisk Start, innym – komunikat "Czekaj na start".
    /// </summary>
    private void SetupStartUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
            startButton.onClick.AddListener(BeginGame);
        }
        else
        {
            waitingText.SetActive(true);
        }
    }

    /// <summary>
    /// Wywoływane przez MasterClienta – RPC do wszystkich o starcie wyścigu.
    /// </summary>
    public void BeginGame()
    {
        photonView.RPC(nameof(StartGame), RpcTarget.All);
    }

    [PunRPC]
    private void StartGame()
    {
        // Schowaj UI startowe
        waitingText.SetActive(false);
        startButton.gameObject.SetActive(false);

        // Rozpocznij odliczanie
        InvokeRepeating(nameof(CountDown), 1f, 1f);
    }

    private void CountDown()
    {
        startText.gameObject.SetActive(true);

        if (timer > 0)
        {
            startText.text = timer.ToString();
            audioSource.PlayOneShot(countSound);
            timer--;
        }
        else
        {
            startText.text = "START!";
            audioSource.PlayOneShot(startSound);

            isRacing = true;
            CancelInvoke(nameof(CountDown));
            Invoke(nameof(HideStartText), 1f);

            // Po starcie zbierz wszystkie CheckpointController’y
            SetupCheckpointControllers();
        }
    }

    private void HideStartText()
    {
        startText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Tworzy samochód lokalnego gracza dopiero po wejściu do pokoju.
    /// </summary>
    private GameObject SpawnLocalCar()
    {
        // ActorNumber zaczyna się od 1, więc idx = ActorNumber-1
        int idx = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 pos = spawnPositions[idx].position;
        Quaternion rot = spawnPositions[idx].rotation;

        // Dane do CarAppearance lub OnlinePlayer
        object[] instData = new object[]
        {
            PlayerPrefs.GetString("PlayerName"),
            PlayerPrefs.GetInt("Red"),
            PlayerPrefs.GetInt("Green"),
            PlayerPrefs.GetInt("Blue")
        };

        // Instantiate z Photonem – wysyła do wszystkich event
        GameObject car = PhotonNetwork.Instantiate(
            carPrefab.name,
            pos, rot,
            0,
            instData
        );

        // Włącz lokalne sterowanie
        car.GetComponent<DrivingScript>().enabled = true;
        car.GetComponent<PlayerController>().enabled = true;

        // Kamera podąża tylko za hostem (MasterClient)
        if (PhotonNetwork.IsMasterClient)
        {
            var camCtrl = FindFirstObjectByType<CameraController>();
            if (camCtrl != null)
            {
                camCtrl.SetCameraProperties(car);
            }
        }

        return car;
    }

    /// <summary>
    /// Zbiera wszystkie CheckpointController’y z tagiem "Car" do sprawdzania mety.
    /// </summary>
    private void SetupCheckpointControllers()
    {
        var cars = GameObject.FindGameObjectsWithTag("Car");
        carsControllers = new CheckpointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsControllers[i] = cars[i].GetComponent<CheckpointController>();
        }
    }

    void LateUpdate()
    {
        // Jeśli wyścig trwa i mamy kontrolery – sprawdzamy, czy wszyscy ukończyli
        if (!isRacing || carsControllers == null) return;

        int finished = 0;
        foreach (var ctrl in carsControllers)
        {
            if (ctrl.lap > totalLaps)
                finished++;
        }

        if (finished == carsControllers.Length)
        {
            isRacing = false;
            endRacePanel.SetActive(true);
            Debug.Log("Wyścig skończony");
        }
    }
}
