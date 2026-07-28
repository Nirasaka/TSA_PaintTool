using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI input;

    [SerializeField]
    private HideAfterSeconds testSuccess;
    [SerializeField] 
    private HideAfterSeconds testFailure;

    [SerializeField]
    private HideAfterSeconds sendSuccess;
    [SerializeField]
    private HideAfterSeconds sendFailure;

    [SerializeField]
    private RawImage viewer;

    [SerializeField]
    private PaintCanvas paintCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Test()
    {
        StartCoroutine(CheckAdress("http://" + Regex.Replace(input.text, @"[^0-9\.]", "") + ":5000/hello"));
    }

    private IEnumerator CheckAdress(string url)
    {
        using var request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            testFailure.Show();
        }
        else
        {
            testSuccess.Show();
        }
    }

    public void CheckImg()
    {
        if (paintCanvas.IsNotPainted())
        {
            Debug.LogError("‰æ‘œ‚ª‹ó‚Å‚·");
            return;
        }
        StartCoroutine(CheckImgCoroutine("http://" + Regex.Replace(input.text, @"[^0-9\.]", "") + ":5000/checkimg"));
    }

    private IEnumerator CheckImgCoroutine(string url)
    {
        WWWForm form = new WWWForm();

        var dt = DateTime.Now;

        string filename = dt.Year.ToString() + dt.Month.ToString()
                        + dt.Day.ToString() + dt.Hour.ToString()
                        + dt.Minute.ToString() + dt.Second.ToString()
                        + dt.Millisecond.ToString() + ".png";

        form.AddBinaryData(
            "image",
            paintCanvas.GetPNG(),
            filename,
            "image/png");

        using UnityWebRequest request = UnityWebRequest.Post(url, form);

        request.downloadHandler = new DownloadHandlerTexture();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload Success");

            Texture2D img = DownloadHandlerTexture.GetContent(request);

            viewer.texture = img;

            request.Dispose();
        }
        else
        {
            Debug.LogError(request.error);
        }
    }


    public void SendImg()
    {
        if (paintCanvas.IsNotPainted())
        {
            Debug.LogError("‰æ‘œ‚ª‹ó‚Å‚·");
            return;
        }
        StartCoroutine(SendImgCoroutine("http://" + Regex.Replace(input.text, @"[^0-9\.]", "") + ":5000/addqueue"));
    }

    private IEnumerator SendImgCoroutine(string url)
    {
        WWWForm form = new WWWForm();

        var dt = DateTime.Now;

        string filename = dt.Year.ToString() + dt.Month.ToString()
                        + dt.Day.ToString() + dt.Hour.ToString()
                        + dt.Minute.ToString() + dt.Second.ToString()
                        + dt.Millisecond.ToString() + ".png";

        form.AddBinaryData(
            "image",
            paintCanvas.GetPNG(),
            filename,
            "image/png");

        using UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            sendFailure.Show();
        }
        else
        {
            sendSuccess.Show();
        }
    }
}
