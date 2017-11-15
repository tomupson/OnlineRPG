using UnityEngine;

public class PauseOptionsHandler : MonoBehaviour
{
    public void ApplyButtonClicked_Graphics()
    {
        GraphicsManager.singleton.ApplySettings();
    }

    public void ResetButtonClicked_Graphics()
    {
        GraphicsManager.singleton.ResetGraphicsSettings();
    }

    public void ApplyButtonClicked_Controls()
    {

    }

    public void ResetButtonClicked_Controls()
    {
        InputManager.singleton.ResetKeybinds();
    }

    public void ApplyButtonClicked_Game()
    {
        GeneralOptionsManager.singleton.ApplySettings();
    }

    public void ResetButtonClicked_Game()
    {
        GeneralOptionsManager.singleton.ResetSettings();
    }

    public void ApplyButtonClicked_Audio()
    {
        AudioManager.singleton.ApplySettings();
    }

    public void ResetButtonClicked_Audio()
    {
        AudioManager.singleton.ResetAudioSettings();
    }
}