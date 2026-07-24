using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ColorPaletteButton : MonoBehaviour
{
    [SerializeField]
    private PaintToolManager toolManager;

    private Button button;
    private Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        toolManager.SetColor(image.color);
    }
}