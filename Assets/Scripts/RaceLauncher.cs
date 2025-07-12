using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RaceLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte maxPlayersPerRoom = 3;
    private bool isConnecting;
    [SerializeField] private Text networkText;
    [SerializeField] private InputField playerNameInput;
    private const string gameVersion = "2.50"; // dopasuj do wersji konsoli/AppVersion

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // Ustaw domyœln¹ wersjê gry w PhotonServerSettings:
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameInput.text = PlayerPrefs.GetString("PlayerName");
            PhotonNetwork.NickName = playerNameInput.text;
        }
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
        PhotonNetwork.NickName = name;
    }

    public void ConnectNetwork()
    {
        networkText.text = "";
        isConnecting = true;
        PhotonNetwork.NickName = playerNameInput.text;

        if (PhotonNetwork.IsConnected)
        {
            networkText.text += "Joining Room...\n";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            networkText.text += "Connecting...\n";
            // wersja gry ju¿ ustawiona w Awake, wiêc nie przekazujemy parametru
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            networkText.text += "OnConnectedToMaster...\n";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        networkText.text += "Failed to join random room.\n";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        networkText.text += $"Disconnected: {cause}\n";
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        networkText.text = $"Joined Room with {PhotonNetwork.CurrentRoom.PlayerCount} players.\n";
        PhotonNetwork.LoadLevel("KartTest"); // nazwê sceny dostosuj do swojego projektu
    }
}
