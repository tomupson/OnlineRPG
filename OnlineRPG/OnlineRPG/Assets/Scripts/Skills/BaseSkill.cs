using UnityEngine;

[System.Serializable]
public class BaseSkill : XPSystem
{
    [Header("Skill Info")]
    [Tooltip("Name of the Skill")] public string Name;
    [TextArea(5, 10)] [Tooltip("Description of the Skill")] public string Description;
    [Tooltip("The Icon of the Skill")] public Sprite icon;
}