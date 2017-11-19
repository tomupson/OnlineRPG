using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OptionType : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private bool isDefault = false;

    [SerializeField] private GameObject handlerObj;

    [HideInInspector] public IOptionHandler handler;
    [HideInInspector] public bool isActiveType = true;

    void Start()
    {
        handler = handlerObj.GetComponent<IOptionHandler>();

        if (!isDefault)
            Hide();
    }

    public void Show()
    {
        OptionType currentType = FindObjectsOfType<OptionType>().Where(x => x.isActiveType).FirstOrDefault();
        bool errors = currentType.CheckForErrors();
        if (errors)
        {
            PopupHandler.singleton.CreatePopup(new PopupInfo("Apply changes?", new List<PopupButtonInfo>()
            {
                new PopupButtonInfo("Apply", delegate
                {
                    if (currentType.handler is GraphicsManager)
                    {
                        ((GraphicsManager)currentType.handler).ApplySettings();
                    } else if (currentType.handler is InputManager)
                    {
                        ((InputManager)currentType.handler).ApplySettings();
                    } else if (currentType.handler is GeneralOptionsManager)
                    {
                        ((GeneralOptionsManager)currentType.handler).ApplySettings();
                    } else if (currentType.handler is AudioManager)
                    {
                        ((AudioManager)currentType.handler).ApplySettings();
                    }
                    SwitchTab();
                }),
                new PopupButtonInfo("Cancel", delegate
                {
                    if (currentType.handler is GraphicsManager)
                    {
                        ((GraphicsManager)currentType.handler).CancelChanges();
                    } else if (currentType.handler is InputManager)
                    {
                        ((InputManager)currentType.handler).CancelChanges();
                    } else if (currentType.handler is GeneralOptionsManager)
                    {
                        ((GeneralOptionsManager)currentType.handler).CancelChanges();
                    } else if (currentType.handler is AudioManager)
                    {
                        ((AudioManager)currentType.handler).CancelChanges();
                    }
                    SwitchTab();
                })
            }));
        }
        else SwitchTab();
    }

    void SwitchTab()
    {
        foreach (OptionType typeTab in FindObjectsOfType<OptionType>())
        {
            typeTab.Hide();
        }

        isActiveType = true;
        myPanel.SetActive(true);
    }

    public void Hide()
    {
        isActiveType = false;
        myPanel.SetActive(false);
    }

    public bool CheckForErrors()
    {
        if (handler is GraphicsManager)
        {
            return ((GraphicsManager)handler).ChangesAreAwaiting();
        }
        else if (handler is InputManager)
        {
            return ((InputManager)handler).ChangesAreAwaiting();
        }
        else if (handler is GeneralOptionsManager)
        {
            return ((GeneralOptionsManager)handler).ChangesAreAwaiting();
        }
        else if (handler is AudioManager)
        {
            return ((AudioManager)handler).ChangesAreAwaiting();
        }

        return false;
    }
}