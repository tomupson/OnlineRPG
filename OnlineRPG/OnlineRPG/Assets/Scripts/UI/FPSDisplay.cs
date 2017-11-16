using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private GameObject fpsPanel;
    [SerializeField] private TMP_Text fpsText;

    float deltaTime = 0.0f;
    bool showing = false;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (showing)
            DrawFps();
    }

    void DrawFps()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }

    public void Show()
    {
        fpsPanel.SetActive(true);
        showing = true;
    }
    
    public void Hide()
    {
        fpsPanel.SetActive(false);
        showing = false;
    }
}