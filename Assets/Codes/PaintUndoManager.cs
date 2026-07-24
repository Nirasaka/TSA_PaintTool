using System.Collections.Generic;
using UnityEngine;

public class PaintUndoManager : MonoBehaviour
{
    [SerializeField]
    private PaintCanvas paintCanvas;

    [SerializeField]
    [Min(1)]
    private int maxUndo = 20;

    private Stack<Color[]> undoStack = new Stack<Color[]>();

    /// 現在の状態を保存
    public void Save()
    {
        if (paintCanvas == null)
            return;

        // 現在のキャンバスをコピー
        Color[] pixels = (Color[])paintCanvas.GetPixels().Clone();

        undoStack.Push(pixels);

        // 保存数制限
        if (undoStack.Count > maxUndo)
        {
            // Stackの一番古いものを削除
            Color[][] temp = undoStack.ToArray();

            undoStack.Clear();

            for (int i = temp.Length - 2; i >= 0; i--)
            {
                undoStack.Push(temp[i]);
            }
        }
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;

        Color[] pixels = undoStack.Pop();

        paintCanvas.SetPixels(pixels);
    }

    /// Undo履歴を削除
    public void ClearHistory()
    {
        undoStack.Clear();
    }

    /// Undo可能かどうか
    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }

    /// 現在の履歴数
    public int UndoCount()
    {
        return undoStack.Count;
    }
}