using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Range(4, 100)]
    private int brushSize = 8;

    [Header("ツールボタン")]
    [SerializeField]
    private Button pen;
    [SerializeField]
    private Button filler;
    [SerializeField]
    private Button eraser;
    [SerializeField]
    private Button eyedropper;

    private Dictionary<ToolType, Button> toolButtons = new();

    public ToolType CurrentTool => currentTool;
    public Color CurrentColor => currentColor;
    public int BrushSize => brushSize;

    /// ツール変更通知
    public event Action<ToolType> OnToolChanged;

    /// 色変更通知
    public event Action<Color> OnColorChanged;

    /// 太さ変更通知
    public event Action<int> OnBrushSizeChanged;


    private void Start()
    {
        toolButtons[ToolType.Pen] = pen;
        toolButtons[ToolType.Fill] = filler;
        toolButtons[ToolType.Eraser] = eraser;
        toolButtons[ToolType.Eyedropper] = eyedropper;

        SetInteractableFalse(currentTool);
    }


    public void SetTool(ToolType tool)
    {
        if (currentTool == tool)
            return;

        currentTool = tool;
        OnToolChanged?.Invoke(tool);
    }

    public void SetInteractableFalse(ToolType tool)
    {
        foreach(var button in toolButtons)
        {
            if(button.Value == toolButtons[tool])
            {
                button.Value.interactable = false;
            }
            else
            {
                button.Value.interactable = true;
            }
        }
    }

    public void SetPen()
    {
        SetTool(ToolType.Pen);
        SetInteractableFalse(ToolType.Pen);
    }

    public void SetFill()
    {
        SetTool(ToolType.Fill);
        SetInteractableFalse(ToolType.Fill);
    }

    public void SetEraser()
    {
        SetTool(ToolType.Eraser);
        SetInteractableFalse(ToolType.Eraser);
    }

    public void SetEyedropper()
    {
        SetTool(ToolType.Eyedropper);
        SetInteractableFalse(ToolType.Eyedropper);
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