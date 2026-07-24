using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HueSlider : MonoBehaviour
{
    [SerializeField]
    private ColorPicker colorPicker;

    private Slider slider;

    private bool ignoreCallback = false;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void Start()
    {
        slider.value = colorPicker.Hue;

        colorPicker.OnColorChanged += OnColorChanged;
    }

    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.OnColorChanged -= OnColorChanged;
    }

    private void OnSliderChanged(float value)
    {
        if (ignoreCallback)
            return;

        colorPicker.SetHue(value);
    }

    private void OnColorChanged(Color color)
    {
        ignoreCallback = true;

        slider.value = colorPicker.Hue;

        ignoreCallback = false;
    }
}