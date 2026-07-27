using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableWritingScript : InteractableAnimation
{
    protected override void OnEnable()
    {
        animatedObj.SetActive(isActive);
        IntroSceneMenuUI.settingsOpened += HandleReset;
        IntroSceneMenuUI.menuOpened += HandleReset;
    }

    private void OnDisable()
    {
        IntroSceneMenuUI.settingsOpened -= HandleReset;
        IntroSceneMenuUI.menuOpened -= HandleReset;
    }

    private void HandleReset(float value)
    {
        isActive = false;
        animatedObj.SetActive(isActive);
    }
}
