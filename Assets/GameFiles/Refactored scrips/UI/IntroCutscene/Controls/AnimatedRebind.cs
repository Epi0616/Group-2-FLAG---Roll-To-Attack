using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedRebind : ReBindButton, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected Image scribble;
    [SerializeField] protected AnimationOnDemandManager animationManager;

    protected bool isActive = false;

    protected virtual void Start()
    {
        animationManager.Initialize();
        animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.01f);
        SetTargetAlpha(0f);
    }

    public override void ReBind()
    {
        base.ReBind();
    }

    protected override void PlayCompleteAnimation()
    {
        UpdateText("");
        isActive = true;
        SetTargetAlpha(0.8f);
        StartCoroutine(DelayTextShow());
    }

    protected virtual IEnumerator DelayTextShow()
    {
        animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.25f);
        yield return new WaitForSeconds(0.25f);
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }

    protected override IEnumerator EnableAction()
    {
        yield return base.EnableAction();
    }

    public override void ResetBinding()
    {
        base.ResetBinding();
        isActive = false;
        SetTargetAlpha(0f);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SetTargetAlpha(0.4f);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SetTargetAlpha(isActive? 0.8f : 0f);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        SetTargetAlpha(0f);
        ReBind();
    }

    private void SetTargetAlpha(float alpha)
    {
        Color temp = scribble.color;
        temp.a = alpha;
        scribble.color = temp;
    }
}
