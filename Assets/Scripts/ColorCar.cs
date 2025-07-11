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
        // Je¿eli ktoœ zapomnia³ przypisaæ w Inspectorze
        if (_renderer == null) _renderer = GetComponent<Renderer>();

        // Subskrybuj eventy sliderów
        _redSlider.onValueChanged.AddListener(OnSliderChanged);
        _greenSlider.onValueChanged.AddListener(OnSliderChanged);
        _blueSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void Start()
    {
        // Wczytaj zapisane wartoœci lub daj domyœln¹ 128
        int r = PlayerPrefs.GetInt(PREF_RED, 128);
        int g = PlayerPrefs.GetInt(PREF_GREEN, 128);
        int b = PlayerPrefs.GetInt(PREF_BLUE, 128);

        // Ustaw pocz¹tkowe wartoœci sliderów i tekstów
        _redSlider.value = r;
        _greenSlider.value = g;
        _blueSlider.value = b;

        UpdateCarColor(r, g, b);
    }

    private void OnSliderChanged(float _)
    {
        // Gdy zmieni siê którykolwiek z sliderów, pobierz wszystkie wartoœci
        int r = Mathf.RoundToInt(_redSlider.value);
        int g = Mathf.RoundToInt(_greenSlider.value);
        int b = Mathf.RoundToInt(_blueSlider.value);

        // Zaktualizuj kolor i UI
        UpdateCarColor(r, g, b);
        UpdateValueTexts(r, g, b);

        // Zapisz do PlayerPrefs
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
