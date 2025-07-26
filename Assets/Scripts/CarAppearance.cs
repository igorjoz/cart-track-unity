using TMPro;
using UnityEngine;
using Photon.Pun;

public class CarAppearance : MonoBehaviour
{
    public string playerName;
    public Color carColor;
    public TMP_Text nameText;
    public Renderer carRenderer;
    public int playerNumber;


    int carRego;
    bool regoSet = false;
    public CheckpointController checkpoint;

    void Start()
    {
        // Automatycznie znajdź CheckpointController na tym samym obiekcie
        if (checkpoint == null)
        {
            checkpoint = GetComponent<CheckpointController>();
        }

        // Jeśli to jest gra sieciowa, OnlinePlayer zajmie się wyglądem
        if (PhotonNetwork.IsConnected && GetComponent<PhotonView>() != null)
        {
            return; // OnlinePlayer script will handle appearance
        }

        // Kod dla gry lokalnej/pojedynczej
        if (playerNumber == 0)
        {
            playerName = PlayerPrefs.GetString("PlayerName");
            carColor = ColorCar.IntToColor(PlayerPrefs.GetInt("Red"), PlayerPrefs.GetInt("Green"), PlayerPrefs.GetInt("Blue"));
        }
        else
        {
            playerName = "Random " + playerNumber;
            carColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        nameText.text = playerName;
        carRenderer.material.color = carColor;
        nameText.color = carColor;
    }

    private void LateUpdate()
    {
        // Only handle leaderboard in local/single player mode
        if (PhotonNetwork.IsConnected && GetComponent<PhotonView>() != null)
        {
            return; // Skip leaderboard updates in multiplayer - should be handled elsewhere
        }

        if (!regoSet)
        {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }

        // Sprawdź czy checkpoint jest przypisany przed użyciem
        if (checkpoint != null)
        {
            Leaderboard.SetPosition(carRego, checkpoint.lap, checkpoint.checkpoint);
        }
    }
}
