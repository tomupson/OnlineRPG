using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuTab : MonoBehaviour
{
    [SerializeField] private Color highlightColour;
    [SerializeField] private bool isDefault = false;

    private List<IOptionHandler> handlers;

    private Color normalColour;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        normalColour = image.color;
    }

    void Start()
    {
        // TODO: Fix this stuff
        //handlers = FindObjectsOfType<IOptionHandler>();

        if (isDefault)
        {
            Highlight();
        }
    }

    public void Highlight()
    {
        SwitchHighlight(); // <-- NOTE: This gets removed once I uncomment everything else.

        //if (handler is GraphicsManager)
        //{
        //    Debug.Log(((GraphicsManager)handler).ChangesAreAwaiting());
        //    if (!((GraphicsManager)handler).ChangesAreAwaiting())
        //    {
        //        SwitchHighlight();
        //        return;
        //    }
        //}
        //else if (handler is InputManager)
        //{
        //    Debug.Log(((InputManager)handler).ChangesAreAwaiting());
        //    if (!((InputManager)handler).ChangesAreAwaiting())
        //    {
        //        SwitchHighlight();
        //        return;
        //    }
        //}
        //else if (handler is GeneralOptionsManager)
        //{
        //    Debug.Log(((GeneralOptionsManager)handler).ChangesAreAwaiting());
        //    if (!((GeneralOptionsManager)handler).ChangesAreAwaiting())
        //    {
        //        SwitchHighlight();
        //        return;
        //    }
        //}
        //else if (handler is AudioManager)
        //{
        //    Debug.Log(((AudioManager)handler).ChangesAreAwaiting());
        //    if (!((AudioManager)handler).ChangesAreAwaiting())
        //    {
        //        SwitchHighlight();
        //        return;
        //    }
        //}

        //PopupHandler.singleton.CreatePopup(new PopupInfo("Apply changes?", new List<PopupButtonInfo>()
        //{
        //    new PopupButtonInfo("Apply", delegate
        //    {
        //        if (handler is GraphicsManager)
        //        {
        //            ((GraphicsManager)handler).ApplySettings();
        //            SwitchHighlight();
        //        } else if (handler is InputManager)
        //        {
        //            ((InputManager)handler).ApplySettings();
        //            SwitchHighlight();
        //        } else if (handler is GeneralOptionsManager)
        //        {
        //            ((GeneralOptionsManager)handler).ApplySettings();
        //            SwitchHighlight();
        //        } else if (handler is AudioManager)
        //        {
        //            ((AudioManager)handler).ApplySettings();
        //            SwitchHighlight();
        //        }
        //    }),
        //    new PopupButtonInfo("Cancel", delegate
        //    {
        //        if (handler is GraphicsManager)
        //        {
        //            ((GraphicsManager)handler).CancelChanges();
        //        } else if (handler is InputManager)
        //        {
        //            ((InputManager)handler).CancelChanges();
        //        } else if (handler is GeneralOptionsManager)
        //        {
        //            ((GeneralOptionsManager)handler).CancelChanges();
        //        } else if (handler is AudioManager)
        //        {
        //            ((AudioManager)handler).CancelChanges();
        //        }
        //    })
        //}));
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