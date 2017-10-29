using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Option
{
    [TextArea]public string text;
    public UnityEvent onClick;
}