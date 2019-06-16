using UnityEngine;

[RequireComponent(typeof(PostProcessingBehaviour), typeof(Camera))]
public class PostProcessingInitializer : MonoBehaviour
{
    PostProcessingBehaviour ppBehaviour;

    GraphicsManager graphicsMan;

    void Start()
    {
        ppBehaviour = GetComponent<PostProcessingBehaviour>();

        graphicsMan = GraphicsManager.singleton;

        // Motion Blur
        ppBehaviour.profile.motionBlur.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("MOTION_BLUR")).IsChecked;
        ppBehaviour.profile.motionBlur.settings = new MotionBlurModel.Settings()
        {
            shutterAngle = ((float)((SliderInfo)graphicsMan.GetSetting("MOTION_BLUR_AMOUNT")).Value / 100.0f) * 360.0f,
            frameBlending = MotionBlurModel.Settings.defaultSettings.frameBlending,
            sampleCount = MotionBlurModel.Settings.defaultSettings.sampleCount
        };

        ppBehaviour.profile.ambientOcclusion.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("AMBIENT_OCCLUSION")).IsChecked;
        ppBehaviour.profile.bloom.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("BLOOM")).IsChecked;

        EventHandler.OnGraphicsSettingsChanged += CheckForChange;
    }

    void CheckForChange()
    {
        ppBehaviour.profile.motionBlur.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("MOTION_BLUR")).IsChecked;
        ppBehaviour.profile.motionBlur.settings = new MotionBlurModel.Settings()
        {
            shutterAngle = ((float)((SliderInfo)graphicsMan.GetSetting("MOTION_BLUR_AMOUNT")).Value / 100.0f) * 360.0f,
            frameBlending = MotionBlurModel.Settings.defaultSettings.frameBlending,
            sampleCount = MotionBlurModel.Settings.defaultSettings.sampleCount
        };

        ppBehaviour.profile.ambientOcclusion.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("AMBIENT_OCCLUSION")).IsChecked;
        ppBehaviour.profile.bloom.enabled = (bool)((ToggleInfo)graphicsMan.GetSetting("BLOOM")).IsChecked;
    }
}