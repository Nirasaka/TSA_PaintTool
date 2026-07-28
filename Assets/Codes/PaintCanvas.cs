using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintCanvas : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private RawImage rawImage;

    [SerializeField]
    public Color backgroundColor = Color.white;

    private Texture2D texture;

    public Texture2D Texture => texture;

    public int Width => texture.width;
    public int Height => texture.height;

    private void Awake()
    {
        Rect rect = rawImage.rectTransform.rect;

        texture = new Texture2D(
            Mathf.RoundToInt(rect.width),
            Mathf.RoundToInt(rect.height),
            TextureFormat.RGBA32,
            false);

        texture.filterMode = FilterMode.Point;

        rawImage.texture = texture;

        Clear();
    }

    #region Coordinate

    public bool ScreenToTexture(Vector2 screenPos, out Vector2 texPos)
    {
        RectTransform rectTransform = rawImage.rectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPos,
            null,
            out Vector2 local))
        {
            texPos = Vector2.zero;
            return false;
        }

        Rect rect = rectTransform.rect;

        float x = Mathf.InverseLerp(rect.xMin, rect.xMax, local.x);
        float y = Mathf.InverseLerp(rect.yMin, rect.yMax, local.y);

        texPos = new Vector2(
            x * texture.width,
            y * texture.height);

        return true;
    }

    #endregion

    #region Draw

    public void DrawLine(Vector2 from, Vector2 to, Color color, int size)
    {
        float dist = Vector2.Distance(from, to);

        if (dist < 1f)
        {
            DrawBrush(from, color, size);
            texture.Apply();
            return;
        }

        int steps = Mathf.CeilToInt(dist);

        for (int i = 0; i <= steps; i++)
        {
            Vector2 p = Vector2.Lerp(from, to, i / (float)steps);

            DrawBrush(p, color, size);
        }

        texture.Apply();
    }

    public void DrawBrush(Vector2 center, Color color, int size)
    {
        int radius = Mathf.Max(1, size / 2);

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

                if (px < 0 || px >= texture.width)
                    continue;

                if (py < 0 || py >= texture.height)
                    continue;

                texture.SetPixel(px, py, color);
            }
        }
    }

    #endregion

    #region Fill

    public void FloodFill(Vector2 pos, Color color)
    {
        int startX = Mathf.RoundToInt(pos.x);
        int startY = Mathf.RoundToInt(pos.y);

        if (startX < 0 || startX >= texture.width)
            return;

        if (startY < 0 || startY >= texture.height)
            return;

        Color target = texture.GetPixel(startX, startY);

        if (target.Equals(color))
            return;

        Queue<Vector2Int> queue = new();

        queue.Enqueue(new Vector2Int(startX, startY));

        while (queue.Count > 0)
        {
            Vector2Int p = queue.Dequeue();

            if (p.x < 0 || p.x >= texture.width)
                continue;

            if (p.y < 0 || p.y >= texture.height)
                continue;

            if (texture.GetPixel(p.x, p.y) != target)
                continue;

            texture.SetPixel(p.x, p.y, color);

            queue.Enqueue(new Vector2Int(p.x + 1, p.y));
            queue.Enqueue(new Vector2Int(p.x - 1, p.y));
            queue.Enqueue(new Vector2Int(p.x, p.y + 1));
            queue.Enqueue(new Vector2Int(p.x, p.y - 1));
        }

        texture.Apply();
    }

    #endregion

    #region Utility

    public void Clear()
    {
        Color[] pixels = new Color[texture.width * texture.height];

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = backgroundColor;

        texture.SetPixels(pixels);
        texture.Apply();
    }

    public byte[] GetPNG()
    {
        Texture2D exportTexture = new Texture2D(
                                    texture.width, 
                                    texture.height, 
                                    TextureFormat.RGBA32, 
                                    false);

        Color[] pixels = texture.GetPixels();

        for(int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == backgroundColor)
            {
                pixels[i] = Color.white;
            }
        }

        exportTexture.SetPixels(pixels);
        exportTexture.Apply();

        byte[] png = exportTexture.EncodeToPNG();

        Destroy(exportTexture);

        return png;
    }

    public Color GetPixel(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);

        if (x < 0 || x >= texture.width ||
            y < 0 || y >= texture.height)
        {
            return backgroundColor;
        }

        return texture.GetPixel(x, y);
    }

    public Color[] GetPixels()
    {
        return texture.GetPixels();
    }

    public void SetPixels(Color[] pixels)
    {
        texture.SetPixels(pixels);
        texture.Apply();
    }

    public void SetTexture(Texture2D tex)
    {
        texture = new Texture2D(
            tex.width,
            tex.height,
            TextureFormat.RGBA32,
            false);

        texture.SetPixels(tex.GetPixels());
        texture.Apply();

        rawImage.texture = texture;
    }

    public bool IsNotPainted()
    {
        foreach(Color c in GetPixels())
        {
            if(c != backgroundColor)
            {
                return false;
            }
        }
        return true;
    }

    #endregion
}