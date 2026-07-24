using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorHistoryManager : MonoBehaviour
{
    [SerializeField]
    private PaintToolManager toolManager;

    [SerializeField]
    private List<Image> historyImages = new();

    [SerializeField]
    private int maxHistory = 10;

    private readonly List<Color> history = new();

    private void Start()
    {
        RefreshUI();
    }

    public void AddColor(Color color)
    {
        history.Remove(color);

        history.Insert(0, color);

        if (history.Count > maxHistory)
            history.RemoveAt(history.Count - 1);

        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < historyImages.Count; i++)
        {
            if (i < history.Count)
            {
                historyImages[i].gameObject.SetActive(true);
                historyImages[i].color = history[i];
            }
            else
            {
                historyImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectHistoryColor(int index)
    {
        if (index < 0 || index >= history.Count)
            return;

        toolManager.SetColor(history[index]);
    }
}