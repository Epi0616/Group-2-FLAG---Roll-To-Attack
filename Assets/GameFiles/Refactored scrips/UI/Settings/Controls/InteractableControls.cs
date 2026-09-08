using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableControls : InteractableTickBox, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected override void Start()
    {
        base.Start();
        SetAlpha(0);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.25f);
        SetAlpha(0.6f);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(0);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        SetAlpha(0);
    }
}
