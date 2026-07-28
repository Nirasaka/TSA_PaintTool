using UnityEngine;
using UnityEngine.EventSystems;

public class PaintInputController : MonoBehaviour
{
    [SerializeField]
    private PaintCanvas paintCanvas;

    [SerializeField]
    private PaintToolManager toolManager;

    [SerializeField]
    private PaintUndoManager undoManager;

    [SerializeField]
    private ColorHistoryManager historyManager;

    private Vector2 previousPos;
    private bool drawing = false;

    public void OnPointerDown(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;

        if (eventData == null)
            return;

        if (!paintCanvas.ScreenToTexture(eventData.position, out previousPos))
            return;

        // Undo—p‚É•Û‘¶
        if (undoManager != null)
            undoManager.Save();

        drawing = true;

        switch (toolManager.CurrentTool)
        {
            case PaintToolManager.ToolType.Pen:
                historyManager.AddColor(toolManager.CurrentColor);

                paintCanvas.DrawBrush(previousPos, toolManager.CurrentColor, toolManager.BrushSize);

                paintCanvas.Texture.Apply();

                break;

            case PaintToolManager.ToolType.Fill:
                historyManager.AddColor(toolManager.CurrentColor);

                paintCanvas.FloodFill( previousPos, toolManager.CurrentColor);

                drawing = false;

                break;
            case PaintToolManager.ToolType.Eraser:
                paintCanvas.DrawBrush(previousPos, paintCanvas.backgroundColor, toolManager.BrushSize);

                paintCanvas.Texture.Apply();

                break;
            case PaintToolManager.ToolType.Eyedropper:
                Color color = paintCanvas.GetPixel(previousPos);

                toolManager.SetColor(color);

                drawing = false;

                break;
        }
    }

    public void OnDrag(BaseEventData data)
    {
        if (!drawing)
            return;

        PointerEventData eventData = data as PointerEventData;

        if (eventData == null)
            return;

        if (!paintCanvas.ScreenToTexture(eventData.position, out Vector2 currentPos))
            return;

        if (toolManager.CurrentTool == PaintToolManager.ToolType.Pen)
        {
            paintCanvas.DrawLine(previousPos, currentPos, toolManager.CurrentColor, toolManager.BrushSize);

            previousPos = currentPos;
        }
        else if(toolManager.CurrentTool == PaintToolManager.ToolType.Eraser)
        {
            paintCanvas.DrawLine(previousPos, currentPos, paintCanvas.backgroundColor, toolManager.BrushSize);

            previousPos = currentPos;
        }
    }

    public void OnPointerUp(BaseEventData data)
    {
        drawing = false;
    }

    public void ClearCanvas()
    {
        undoManager.Save();
        paintCanvas.Clear();
    }
}