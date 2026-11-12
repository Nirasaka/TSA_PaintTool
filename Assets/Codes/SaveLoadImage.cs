using System;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System.Collections;

public class SaveLoadImage : MonoBehaviour
{
    [SerializeField]
    private PaintCanvas paintCanvas;

    private void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Image", ".png", ".jpg", ".jpeg"));

        FileBrowser.SetDefaultFilter(".png");

        FileBrowser.HideDialog();

    }

    public void LoadPNG()
    {
        FileBrowser.ShowLoadDialog(OnLoadSuccess, OnCancel, FileBrowser.PickMode.Files, false, null, null, "Open the image", "Open");
    }

    private void OnLoadSuccess(string[] paths)
    {
        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(paths[0]);

        Texture2D texture = new Texture2D(paintCanvas.Width, paintCanvas.Height);

        if (!texture.LoadImage(bytes))
        {
            Debug.LogError("画像読み込み失敗");
            return;
        }

        paintCanvas.SetTexture(texture);
    }

    public void SavePNG()
    {
        string folder = GetDownloadPath();

        var dt = DateTime.Now;

        string filename = dt.Year.ToString() + dt.Month.ToString()
                        + dt.Day.ToString() + dt.Hour.ToString()
                        + dt.Minute.ToString() + dt.Second.ToString()
                        + dt.Millisecond.ToString() + ".png";

        FileBrowser.ShowSaveDialog(OnSaveSuccess, OnCancel, FileBrowser.PickMode.Files, false, folder, filename, "Save the image", "Save");
    }

    public void SavePNG(string filename)
    {
        string folder = GetDownloadPath();
        FileBrowser.ShowSaveDialog(OnSaveSuccess, OnCancel, FileBrowser.PickMode.Files, false, folder, filename, "Save the image", "Save");
    }

    public void SavePNGSilent(string filename)
    {
        string folder = GetDownloadPath();
        string path = Path.Combine(folder, filename);
        var png = paintCanvas.GetPNG(false);
        File.WriteAllBytes(path, png);
        Debug.Log("画像を保存：" + path);
    }

    private void OnSaveSuccess(string[] paths)
    {
        var png = paintCanvas.GetPNG(false);

        File.WriteAllBytes(paths[0], png);

        Debug.Log("画像を保存：" + paths[0]);
    }

    private string GetDownloadPath()
    {
#if UNITY_STANDALONE_WIN
        return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
#elif UNITY_ANDROID
        return "/storage/emulated/0/Download";
#else
        return Application.persistentDataPath;
#endif
    }

    private void OnCancel()
    {
        Debug.Log("ファイル読み書きがキャンセルされました");
    }
}
