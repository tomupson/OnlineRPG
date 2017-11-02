using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Option
{
    //public delegate void OnOptionClickDelegate();
    [TextArea] public string text;
    public UnityEvent onOptionClick;
}