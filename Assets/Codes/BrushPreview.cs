using UnityEngine;
using UnityEngine.UI;

public class BrushPreview : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PaintToolManager toolManager;

    [SerializeField]
    private Image previewImage;

    [Header("Preview Size")]
    [SerializeField]
    private float minDiameter = 8f;

    [SerializeField]
    private float maxDiameter = 50f;

    [SerializeField]
    private int maxBrushSize = 100;

    private void Start()
    {
        toolManager.OnColorChanged += UpdateColor;
        toolManager.OnBrushSizeChanged += UpdateBrushSize;

        // èâä˙ï\é¶
        UpdateColor(toolManager.CurrentColor);
        UpdateBrushSize(toolManager.BrushSize);
    }

    private void OnDestroy()
    {
        if (toolManager == null) return;

        toolManager.OnColorChanged -= UpdateColor;
        toolManager.OnBrushSizeChanged -= UpdateBrushSize;
    }

    private void UpdateColor(Color color)
    {
        previewImage.color = color;
    }

    private void UpdateBrushSize(int size)
    {
        float diameter = Mathf.Lerp(
            minDiameter,
            maxDiameter,
            (float)size / maxBrushSize);

        previewImage.rectTransform.sizeDelta =
            new Vector2(diameter, diameter);
    }
}