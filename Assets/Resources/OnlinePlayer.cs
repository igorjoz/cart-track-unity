using UnityEngine;
using Photon.Pun;
using TMPro;

public class OnlinePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;

    [Header("Appearance Components")]
    public TMP_Text nameText;
    public Renderer carRenderer;

    private int carRego;
    private bool regoSet = false;
    private CheckpointController checkpoint;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
    }

    private void Start()
    {
        // Automatycznie znajdź CheckpointController na tym obiekcie lub dzieciach
        checkpoint = GetComponent<CheckpointController>();
        if (checkpoint == null)
        {
            checkpoint = GetComponentInChildren<CheckpointController>();
        }

        // Zastosuj dane z instantiation - robione w Start() żeby mieć pewność że wszystko jest zainicjalizowane
        ApplyInstantiationData();
    }

    private void LateUpdate()
    {
        // Handle leaderboard registration and updates for multiplayer
        if (!regoSet && nameText != null && !string.IsNullOrEmpty(nameText.text))
        {
            carRego = Leaderboard.RegisterCar(nameText.text);
            regoSet = true;
            return;
        }

        // Update leaderboard position
        if (checkpoint != null && regoSet)
        {
            Leaderboard.SetPosition(carRego, checkpoint.lap, checkpoint.checkpoint);
        }
    }

    private void ApplyInstantiationData()
    {
        // Pobierz dane z instantiation data
        object[] data = photonView.InstantiationData;

        if (data != null && data.Length >= 4)
        {
            string playerName = (string)data[0];
            int red = (int)data[1];
            int green = (int)data[2];
            int blue = (int)data[3];

            // Zastosuj wygląd gracza
            ApplyPlayerAppearance(playerName, red, green, blue);
        }
    }

    private void ApplyPlayerAppearance(string playerName, int red, int green, int blue)
    {
        // Ustaw nazwę gracza
        if (nameText != null)
        {
            nameText.text = playerName;
        }

        // Ustaw kolor samochodu
        if (carRenderer != null)
        {
            Color carColor = ColorCar.IntToColor(red, green, blue);
            carRenderer.material.color = carColor;

            // Ustaw również kolor tekstu na kolor samochodu
            if (nameText != null)
            {
                nameText.color = carColor;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Nie potrzebujemy synchronizacji w czasie rzeczywistym dla wyglądu
    }
}
