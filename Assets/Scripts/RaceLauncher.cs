using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceLauncher : MonoBehaviour
{
    [Header("Assign your TMP Input Field here")]
    public TMP_InputField playerNameField;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            playerNameField.text = PlayerPrefs.GetString("PlayerName");
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}