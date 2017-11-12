using System;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    void Start()
    {
        if (!Directory.Exists($@"{documentsPath}\OnlineRPG"))
        {
            Directory.CreateDirectory($@"{documentsPath}\OnlineRPG");
            Directory.CreateDirectory($@"{documentsPath}\OnlineRPG\Screenshots");
            Directory.CreateDirectory($@"{documentsPath}\OnlineRPG\Logs");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.singleton.GetKey("TAKE_SCREENSHOT").Key))
        {
            DateTime now = DateTime.Now;
            
            Debug.Log($@"Saved Screenshot to: {documentsPath}\OnlineRPG\Screenshots\OnlineRPG Screenshot {now.Day}-{now.Month}-{now.Year.ToString("YY")} {now.Hour}.{now.Minute}.{now.Second}.png");
            ScreenCapture.CaptureScreenshot($@"{documentsPath}\OnlineRPG\Screenshots\OnlineRPG Screenshot {now.Day}-{now.Month}-{now.Year} {now.Hour}.{now.Minute}.{now.Second}.png");
        }
    }
}