using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableWritingScript : InteractableTickBox
{
    protected override void OnEnable()
    {
        animatedWritingObj.SetActive(isActive);
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
        animatedWritingObj.SetActive(isActive);
    }
}
