using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaintController : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [SerializeField]
    private RawImage img;

    [SerializeField]
    private int thickness = 8;

    [SerializeField]
    private Color penColor = Color.black;

    private Texture2D tex;

    private Vector2 previousPos;

    private bool drawing = false;

    private void Start()
    {
        Rect rect = img.rectTransform.rect;

        tex = new Texture2D(
            Mathf.RoundToInt(rect.width),
            Mathf.RoundToInt(rect.height),
            TextureFormat.RGBA32,
            false);

        InitTexture();

        img.texture = tex;
    }

    private void InitTexture()
    {
        Color[] colors = new Color[tex.width * tex.height];

        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.white;

        tex.SetPixels(colors);
        tex.Apply();
    }

    /// <summary>
    /// 画面座標→Texture座標
    /// </summary>
    private bool ScreenToTexturePosition(Vector2 screenPos, out Vector2 texPos)
    {
        RectTransform rectTransform = img.rectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPos,
            null,
            out Vector2 localPos))
        {
            texPos = Vector2.zero;
            return false;
        }

        Rect rect = rectTransform.rect;

        float x = Mathf.InverseLerp(rect.xMin, rect.xMax, localPos.x);
        float y = Mathf.InverseLerp(rect.yMin, rect.yMax, localPos.y);

        texPos = new Vector2(
            x * tex.width,
            y * tex.height);

        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ScreenToTexturePosition(eventData.position, out previousPos))
            return;

        drawing = true;

        DrawBrush(previousPos);

        tex.Apply();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!drawing)
            return;

        if (!ScreenToTexturePosition(eventData.position, out Vector2 currentPos))
            return;

        DrawLine(previousPos, currentPos);

        previousPos = currentPos;

        tex.Apply();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drawing = false;
    }

    /// <summary>
    /// 線を描画
    /// </summary>
    private void DrawLine(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);

        if (distance < 1f)
        {
            DrawBrush(end);
            return;
        }

        int steps = Mathf.CeilToInt(distance);

        for (int i = 0; i <= steps; i++)
        {
            Vector2 pos = Vector2.Lerp(start, end, (float)i / steps);
            DrawBrush(pos);
        }
    }

    /// <summary>
    /// ブラシ描画
    /// </summary>
    private void DrawBrush(Vector2 center)
    {
        int radius = thickness / 2;

        int cx = Mathf.RoundToInt(center.x);
        int cy = Mathf.RoundToInt(center.y);

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y > radius * radius)
                    continue;

                int px = cx + x;
                int py = cy + y;

                if (px < 0 || px >= tex.width)
                    continue;

                if (py < 0 || py >= tex.height)
                    continue;

                tex.SetPixel(px, py, penColor);
            }
        }
    }

    /// <summary>
    /// キャンバス取得（PNG保存用）
    /// </summary>
    public Texture2D GetTexture()
    {
        return tex;
    }

    /// <summary>
    /// PNG取得（アップロード用）
    /// </summary>
    public byte[] GetPNG()
    {
        return tex.EncodeToPNG();
    }

    /// <summary>
    /// 全消し
    /// </summary>
    public void Clear()
    {
        InitTexture();
    }
}