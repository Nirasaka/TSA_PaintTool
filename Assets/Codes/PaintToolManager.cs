using System;
using UnityEngine;

public class PaintToolManager : MonoBehaviour
{
    public enum ToolType
    {
        Pen,
        Fill,
        Eraser,
        Eyedropper
    }

    [Header("Current Tool")]
    [SerializeField]
    private ToolType currentTool = ToolType.Pen;

    [Header("Brush")]
    [SerializeField]
    private Color currentColor = Color.black;

    [SerializeField]
    [Range(1, 100)]
    private int brushSize = 8;

    public ToolType CurrentTool => currentTool;
    public Color CurrentColor => currentColor;
    public int BrushSize => brushSize;

    /// ツール変更通知
    public event Action<ToolType> OnToolChanged;

    /// 色変更通知
    public event Action<Color> OnColorChanged;

    /// 太さ変更通知
    public event Action<int> OnBrushSizeChanged;


    public void SetTool(ToolType tool)
    {
        if (currentTool == tool)
            return;

        currentTool = tool;
        OnToolChanged?.Invoke(tool);
    }

    public void SetPen()
    {
        SetTool(ToolType.Pen);
    }

    public void SetFill()
    {
        SetTool(ToolType.Fill);
    }

    public void SetEraser()
    {
        SetTool(ToolType.Eraser);
    }

    public void SetEyedropper()
    {
        SetTool(ToolType.Eyedropper);
    }


    public void SetColor(Color color)
    {
        currentColor = color;
        OnColorChanged?.Invoke(color);
        OnToolChanged?.Invoke(currentTool);
    }


    public void SetBrushSize(float value)
    {
        SetBrushSize(Mathf.RoundToInt(value));
    }

    public void SetBrushSize(int size)
    {
        size = Mathf.Clamp(size, 1, 100);

        if (brushSize == size)
            return;

        brushSize = size;

        OnBrushSizeChanged?.Invoke(size);
    }


    public void IncreaseBrush()
    {
        SetBrushSize(brushSize + 1);
    }

    public void DecreaseBrush()
    {
        SetBrushSize(brushSize - 1);
    }
}