using System.Collections;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private GameObject maskPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private TMP_Text loginErrorText;

    public static bool loggedIn = false;

    const string loginUrl = @"https://backend.tomupson.xyz/login.php";

    void Start()
    {
        if (!loggedIn)
        {
            maskPanel.SetActive(true);
            loginPanel.SetActive(true);
        }
    }

    public void LoginButtonPressed()
    {
        ClearErrors();

        string errorString = string.Empty;

        if (string.IsNullOrEmpty(usernameInputField.text))
        {
            errorString += "Username cannot be empty.\n";
        }

        if (string.IsNullOrEmpty(passwordInputField.text))
        {
            errorString += "Password cannot be empty.\n";
        }

        if (string.IsNullOrEmpty(errorString))
        {
            StartCoroutine(Login(usernameInputField.text, passwordInputField.text));
        }
        else AddErrors(errorString);
    }

    void ClearErrors()
    {
        loginErrorText.text = "";
    }

    void AddErrors(string errorString)
    {
        loginErrorText.text = errorString;
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("username", username);
        formData.AddField("password", password);

        WWW www = new WWW(loginUrl, formData);

        yield return www;

        Debug.Log(www.text);
    }
}