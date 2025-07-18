using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Renderer))]
public class ColorCar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Slider _redSlider;
    [SerializeField] private Slider _greenSlider;
    [SerializeField] private Slider _blueSlider;
    [SerializeField] private TMP_Text _redValueText;
    [SerializeField] private TMP_Text _greenValueText;
    [SerializeField] private TMP_Text _blueValueText;

    private static readonly string PREF_RED = "Red";
    private static readonly string PREF_GREEN = "Green";
    private static readonly string PREF_BLUE = "Blue";

    private void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();

        _redSlider.onValueChanged.AddListener(OnSliderChanged);
        _greenSlider.onValueChanged.AddListener(OnSliderChanged);
        _blueSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void Start()
    {
        int r = PlayerPrefs.GetInt(PREF_RED, 128);
        int g = PlayerPrefs.GetInt(PREF_GREEN, 128);
        int b = PlayerPrefs.GetInt(PREF_BLUE, 128);

        _redSlider.value = r;
        _greenSlider.value = g;
        _blueSlider.value = b;

        UpdateCarColor(r, g, b);
    }

    private void OnSliderChanged(float _)
    {
        int r = Mathf.RoundToInt(_redSlider.value);
        int g = Mathf.RoundToInt(_greenSlider.value);
        int b = Mathf.RoundToInt(_blueSlider.value);

        UpdateCarColor(r, g, b);
        UpdateValueTexts(r, g, b);

        PlayerPrefs.SetInt(PREF_RED, r);
        PlayerPrefs.SetInt(PREF_GREEN, g);
        PlayerPrefs.SetInt(PREF_BLUE, b);
    }

    private void UpdateCarColor(int red, int green, int blue)
    {
        Color col = new Color(red / 255f, green / 255f, blue / 255f);
        _renderer.material.color = col;
    }

    private void UpdateValueTexts(int red, int green, int blue)
    {
        _redValueText.text = red.ToString();
        _greenValueText.text = green.ToString();
        _blueValueText.text = blue.ToString();
    }

    public static Color IntToColor(int red, int green, int blue)
    {
        float r = (float)red / 255;
        float g = (float)green / 255;
        float b = (float)blue / 255;
        Color col = new Color(r, g, b);
        return col;
    }
}