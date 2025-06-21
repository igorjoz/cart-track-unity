using TMPro;
using UnityEngine;

public class CarAppearance : MonoBehaviour
{
    public string playerName;
    public Color carColor;
    public TMP_Text nameText;
    public Renderer carRenderer;

    void Start()
    {
        nameText.text = playerName;
        carRenderer.material.color = carColor;
        nameText.color = carColor;
    }
}
