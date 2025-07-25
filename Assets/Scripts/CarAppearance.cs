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

    void Start()
    {
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
}
