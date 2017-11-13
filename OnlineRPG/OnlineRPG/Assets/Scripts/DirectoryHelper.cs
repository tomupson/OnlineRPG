using System;
using System.IO;
using UnityEngine;

public class DirectoryHelper : MonoBehaviour
{
    #region Singleton
    public static DirectoryHelper singleton;
    #endregion

    private string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OnlineRPG";

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one Directory Helper on the client!");
            return;
        }

        singleton = this;

        CheckDirectories();
    }

    public void CheckDirectories()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/settings"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/settings");
        }

        if (!Directory.Exists($"{documentsPath}/Screenshots"))
        {
            Directory.CreateDirectory($"{documentsPath}/Screenshots");
        }

        if (!Directory.Exists($"{documentsPath}/Logs"))
        {
            Directory.CreateDirectory($"{documentsPath}/Logs");
        }
    }
}