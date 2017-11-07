using UnityEngine;

[System.Serializable]
public class Option
{
    #region Delegates
    public delegate void OnOptionClickDelegate();
    #endregion

    [TextArea] public string Text;
    public OnOptionClickDelegate OnOptionClick;
}