using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableMovingSlider : Slider, IBeginDragHandler, IEndDragHandler, ILoadPlayerPrefs
{
    public AnimationOnDemandManager animationManager;
    [SerializeField] private PlayerValues audioType;

    protected override void OnEnable()
    {
        base.OnEnable();
        TryLoadPrefs();
    }

    protected override void Start()
    {
        base.Start();
        TryLoadPrefs();
        animationManager?.Initialize();
        animationManager?.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.2f);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        animationManager?.EndCurrentAnimation(MixerType.main);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        AdjustTargetAlpha(1);
        animationManager?.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.35f);
        PlayerPrefsManager.instance?.SetFloat(audioType, value);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        AdjustTargetAlpha(0.6f);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        AdjustTargetAlpha(1);
    }

    private void AdjustTargetAlpha(float alpha)
    {
        if (targetGraphic is Image image)
        {
            Color transparent = image.color;
            transparent.a = alpha;
            image.color = transparent;
        }
    }

    public void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        value = PlayerPrefsManager.instance.GetFloat(audioType);
    }
}
