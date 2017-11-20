using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuTab : MonoBehaviour
{
    [SerializeField] private Color highlightColour;
    [SerializeField] private bool isDefault = false;

    private Color normalColour;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        normalColour = image.color;
    }

    void Start()
    {
        if (isDefault)
        {
            Highlight();
        }
    }

    public void Highlight()
    {
        if (!GraphicsManager.singleton.ChangesAreAwaiting())
        {
            SwitchHighlight();
            return;
        }
        else if (!InputManager.singleton.ChangesAreAwaiting())
        {
            SwitchHighlight();
            return;
        }
        if (!GeneralOptionsManager.singleton.ChangesAreAwaiting())
        {
            SwitchHighlight();
            return;
        }
        if (!AudioManager.singleton.ChangesAreAwaiting())
        {
            SwitchHighlight();
            return;
        }

        PopupHandler.singleton.CreatePopup(new PopupInfo("Apply changes?", new List<PopupButtonInfo>()
        {
            new PopupButtonInfo("Apply", delegate
            {
                GraphicsManager.singleton.ApplySettings();
                InputManager.singleton.ApplySettings();
                GeneralOptionsManager.singleton.ApplySettings();
                AudioManager.singleton.ApplySettings();
                SwitchHighlight();
            }),
            new PopupButtonInfo("Cancel", delegate
            {
                GraphicsManager.singleton.CancelChanges();
                InputManager.singleton.CancelChanges();
                GeneralOptionsManager.singleton.CancelChanges();
                AudioManager.singleton.CancelChanges();
            })
        }));
    }

    public void SwitchHighlight()
    {
        foreach (MenuTab tab in FindObjectsOfType<MenuTab>())
        {
            if (tab != this)
            {
                tab.Unhighlight();
            }
        }

        image.color = highlightColour;
    }

    public void Unhighlight()
    {
        image.color = normalColour;
    }
}