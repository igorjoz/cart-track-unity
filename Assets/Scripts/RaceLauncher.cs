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

    /// <summary>
    /// Hook this up to the OnValueChanged(string) event of your TMP Input Field.
    /// </summary>
    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    /// <summary>
    /// Call this (e.g. from a button) to load the race scene.
    /// </summary>
    public void StartTrial()
    {
        SceneManager.LoadScene(0);
    }
}
