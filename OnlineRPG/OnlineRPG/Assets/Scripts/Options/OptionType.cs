using UnityEngine;
using System.Collections.Generic;

public class OptionType : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private bool isDefault = false;

    void Start()
    {
        if (!isDefault)
            Hide();
    }

    public void Show()
    {
        if (CheckForChanges())
        {
            PopupHandler.singleton.CreatePopup(new PopupInfo("Apply changes?", new List<PopupButtonInfo>()
            {
                new PopupButtonInfo("Apply", delegate
                {
                    GraphicsManager.singleton.ApplySettings();
                    InputManager.singleton.ApplySettings();
                    GeneralOptionsManager.singleton.ApplySettings();
                    AudioManager.singleton.ApplySettings();
                    SwitchTab();
                }),
                new PopupButtonInfo("Cancel", delegate
                {
                    GraphicsManager.singleton.CancelChanges();
                    InputManager.singleton.CancelChanges();
                    GeneralOptionsManager.singleton.CancelChanges();
                    AudioManager.singleton.CancelChanges();
                    SwitchTab();
                })
            }));
        }
        else SwitchTab(); 
    }

    bool CheckForChanges()
    {
        if (GraphicsManager.singleton.ChangesAreAwaiting()) return true;
        if (InputManager.singleton.ChangesAreAwaiting()) return true;
        if (GeneralOptionsManager.singleton.ChangesAreAwaiting()) return true;
        if (AudioManager.singleton.ChangesAreAwaiting()) return true;

        return false;
    }

    void SwitchTab()
    {
        foreach (OptionType typeTab in FindObjectsOfType<OptionType>())
        {
            typeTab.Hide();
        }

        myPanel.SetActive(true);
    }

    public void Hide()
    {
        myPanel.SetActive(false);
    }
}