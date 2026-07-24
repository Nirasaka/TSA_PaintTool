using System;
using System.IO;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    [SerializeField]
    private PaintCanvas paintCanvas;

    public void SavePNG()
    {
        string folder = GetDownloadPath();

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var dt  = DateTime.Now;

        string filename = dt.Year.ToString() + dt.Month.ToString() 
                        + dt.Day.ToString() + dt.Hour.ToString() 
                        + dt.Minute.ToString() + dt.Second.ToString() 
                        + dt.Millisecond.ToString() + ".png";

        string path = Path.Combine(folder, filename);

        var png = paintCanvas.GetPNG();

        File.WriteAllBytes(path, png);

        Debug.Log("âÊëúÇï€ë∂ÅF" + path);
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

}
