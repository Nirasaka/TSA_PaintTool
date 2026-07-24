using UnityEngine;
using UnityEngine.UI;

public class ColorHistoryButton : MonoBehaviour
{
    [SerializeField]
    private ColorHistoryManager historyManager;

    [SerializeField]
    private int index;

    public void OnClick()
    {
        historyManager.SelectHistoryColor(index);
    }
}