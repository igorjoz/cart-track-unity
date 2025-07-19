using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;

public class RaceLauncher : MonoBehaviourPunCallbacks
{
    [Header("Assign your TMP Input Field here")]
    public TMP_InputField playerNameField;

    public TMP_Text networkText;

    public byte maxPlayersPerRoom = 3;

    private string gameVersion = "2.50";

    private bool isConnecting = false;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) {
            playerNameField.text = PlayerPrefs.GetString("PlayerName");
        }

        networkText.text = "Not connected";
    }

    public void SetName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString("PlayerName", name);
            PhotonNetwork.NickName = name;
        }
    }

    public void ConnectNetwork()
    {
        networkText.text = "Connecting...";
        isConnecting = true;

        string savedName = playerNameField.text.Trim();

        if (string.IsNullOrEmpty(savedName))
        {
            networkText.text = "Please enter your nick.";
        }

        SetName(savedName);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            networkText.text = "Connected to Master. Joining room...";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        networkText.text = $"Disconnected: {cause}";
        isConnecting = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        networkText.text = "No random room available. Creating one...";
        // Create a new room with the specified max players
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayersPerRoom
        };
        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnCreatedRoom()
    {
        networkText.text = "Room created. Waiting for players...";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        networkText.text = $"Failed to join room: {message}";
    }

    public override void OnJoinedRoom()
    {
        networkText.text = "Joined room! Loading race scene…";
        // Only the MasterClient will control scene loading
        PhotonNetwork.LoadLevel("Game");
    }
}