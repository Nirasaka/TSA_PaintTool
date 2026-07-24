using System;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private PaintToolManager toolManager;

    private float hue = 0f;
    private float saturation = 1f;
    private float value = 1f;

    public event Action<Color> OnColorChanged;

    public float Hue => hue;
    public float Saturation => saturation;
    public float Value => value;

    private void Start()
    {
        SetColor(toolManager.CurrentColor);
    }

    public void SetHue(float h)
    {
        hue = Mathf.Repeat(h, 1f);
        ApplyColor();
    }

    public void SetSaturation(float s)
    {
        saturation = Mathf.Clamp01(s);
        ApplyColor();
    }

    public void SetValue(float v)
    {
        value = Mathf.Clamp01(v);
        ApplyColor();
    }

    public void SetSV(Vector2 sv)
    {
        saturation = Mathf.Clamp01(sv.x);
        value = Mathf.Clamp01(sv.y);
        ApplyColor();
    }

    public void SetColor(Color color)
    {
        Color.RGBToHSV(color, out hue, out saturation, out value);
        ApplyColor();
    }

    private void ApplyColor()
    {
        Color color = Color.HSVToRGB(hue, saturation, value);

        toolManager.SetColor(color);

        OnColorChanged?.Invoke(color);
    }

    public Color GetColor()
    {
        return Color.HSVToRGB(hue, saturation, value);
    }
}