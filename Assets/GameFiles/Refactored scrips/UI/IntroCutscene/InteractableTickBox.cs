using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableTickBox : MonoBehaviour, ILoadPlayerPrefs, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameObject targetGraphic;
    [SerializeField] protected AnimationOnDemandManager animationManager;
    protected Image image;
    protected bool isActive = false;

    private void OnEnable()
    {
        SetAlpha(1f);
        TryLoadPrefs();
    }

    private void Awake()
    {
        animationManager.Initialize();
        image = targetGraphic.GetComponent<Image>();
    }

    private void Start()
    {
        animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.01f);
        TryLoadPrefs();        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(0.6f);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(isActive? 1 : 0);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isActive = !isActive;

        if (isActive)
        {
            animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.2f);
        }
        Toggle();
        SetAlpha(isActive ? 1 : 0);
    }

    protected void SetAlpha(float alpha)
    {
        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public virtual void Toggle() { }
    public virtual void TryLoadPrefs() { }
}
