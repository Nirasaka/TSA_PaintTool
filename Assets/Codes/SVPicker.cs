using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVPicker : MonoBehaviour
{
    [SerializeField]
    private ColorPicker colorPicker;

    [SerializeField]
    private RawImage svImage;

    [SerializeField]
    private RectTransform cursor;

    private Texture2D texture;

    private const int Resolution = 256;

    private void Start()
    {
        texture = new Texture2D(
            Resolution,
            Resolution,
            TextureFormat.RGBA32,
            false);

        texture.filterMode = FilterMode.Bilinear;

        svImage.texture = texture;

        UpdateTexture();

        colorPicker.OnColorChanged += OnColorChanged;
    }

    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.OnColorChanged -= OnColorChanged;
    }

    private void OnColorChanged(Color c)
    {
        UpdateTexture();

        UpdateCursor(
            colorPicker.Saturation,
            colorPicker.Value);
    }

    public void OnPointerDown(BaseEventData data)
    {
        UpdatePosition((PointerEventData)data);
    }

    public void OnDrag(BaseEventData data)
    {
        UpdatePosition((PointerEventData)data);
    }

    private void UpdatePosition(PointerEventData data)
    {
        RectTransform rect = svImage.rectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            data.position,
            data.pressEventCamera,
            out Vector2 local))
            return;

        Rect r = rect.rect;

        float s = Mathf.InverseLerp(r.xMin, r.xMax, local.x);
        float v = Mathf.InverseLerp(r.yMin, r.yMax, local.y);

        colorPicker.SetSV(new Vector2(s, v));

        UpdateCursor(s, v);
    }

    private void UpdateCursor(float s, float v)
    {
        Rect r = svImage.rectTransform.rect;

        cursor.anchoredPosition = new Vector2(
            Mathf.Lerp(r.xMin, r.xMax, s),
            Mathf.Lerp(r.yMin, r.yMax, v));
    }

    private void UpdateTexture()
    {
        float hue = colorPicker.Hue;

        for (int y = 0; y < Resolution; y++)
        {
            float value = y / (float)(Resolution - 1);

            for (int x = 0; x < Resolution; x++)
            {
                float saturation = x / (float)(Resolution - 1);

                texture.SetPixel(
                    x,
                    y,
                    Color.HSVToRGB(
                        hue,
                        saturation,
                        value));
            }
        }

        texture.Apply();
    }
}