using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;

    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        DrawFps();
    }

    void DrawFps()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }
}