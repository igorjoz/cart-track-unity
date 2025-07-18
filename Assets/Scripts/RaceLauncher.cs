// RaceLauncher.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RaceLauncher : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [Tooltip("TMP Input Field where the player enters their nickname")]
    public TMP_InputField playerNameField;

    [Tooltip("TMP Text for displaying connection status and errors")]
    public TMP_Text networkText;

    [Header("Photon Settings")]
    [Tooltip("Max number of players per room")]
    public byte maxPlayersPerRoom = 3;

    private string gameVersion = "2.50";
    private bool isConnecting = false;

    void Awake()
    {
        // Ensure that all clients in the same room load the same scene
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        // Load saved player name if present
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameField.text = PlayerPrefs.GetString("PlayerName");
        }

        networkText.text = "Not connected";
    }

    /// <summary>
    /// Called by the Input Field OnValueChanged event.
    /// Saves the new player name to PlayerPrefs.
    /// </summary>
    public void SetName(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            PlayerPrefs.SetString("PlayerName", newName);
            PhotonNetwork.NickName = newName;
        }
    }

    /// <summary>
    /// Called by the "Play" button.
    /// Starts the connection process to Photon and joins/creates a room.
    /// </summary>
    public void ConnectNetwork()
    {
        networkText.text = "Connecting...";
        isConnecting = true;

        string savedName = playerNameField.text.Trim();
        if (string.IsNullOrEmpty(savedName))
        {
            networkText.text = "Please enter a name.";
            return;
        }

        PhotonNetwork.NickName = savedName;
        PlayerPrefs.SetString("PlayerName", savedName);

        if (PhotonNetwork.IsConnected)
        {
            // Already connected: try to join a random room
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // Not connected yet: set version and connect
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region Photon Callback Overrides

    public override void OnConnectedToMaster()
    {
        // Only try to join if this was triggered by ConnectNetwork()
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
        // Only the MasterClient will control scene loading, but we set AutomaticallySyncScene
        PhotonNetwork.LoadLevel("Game");
    }

    #endregion
}
