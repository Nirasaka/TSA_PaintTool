using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public void Show()
    {
        menu.SetActive(true);
    }

    public void Hide()
    {
        menu.SetActive(false);
    }

    public void Toggle()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
}
