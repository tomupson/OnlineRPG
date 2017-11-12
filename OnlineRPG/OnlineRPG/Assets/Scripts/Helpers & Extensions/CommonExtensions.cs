using TMPro;
using UnityEngine.UI;

public static class CommonExtensions
{
    public static void Clear(this TMP_InputField inputField)
    {
        inputField.Select();
        inputField.text = "";
    }

    public static void Clear(this InputField inputField)
    {
        inputField.Select();
        inputField.text = "";
    }
}