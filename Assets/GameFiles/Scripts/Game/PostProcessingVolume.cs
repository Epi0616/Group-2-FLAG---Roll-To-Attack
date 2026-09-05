using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private void OnEnable()
    {
        PostProcessingSetting.togglePostProcessing += TogglePostProcessing;
        VideoSettingUI.togglePostProcessing += TogglePostProcessing;
    }

    private void OnDisable()
    {
        PostProcessingSetting.togglePostProcessing -= TogglePostProcessing;
        VideoSettingUI.togglePostProcessing -= TogglePostProcessing;
    }
    private void TogglePostProcessing(bool isActive)
    { 
        volume.weight = isActive ? 1 : 0;
    }
}
