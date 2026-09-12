using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected GameObject animatedObj;
    [SerializeField] protected bool isActive = false;

    protected virtual void OnEnable()
    {
        isActive = animatedObj.activeSelf;
        animatedObj.SetActive(isActive);
    }

    private void Start()
    {
        isActive = animatedObj.activeSelf;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnPointerExit();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown();
    }

    protected virtual void OnPointerEnter() 
    {
        animatedObj?.SetActive(true);
    }
    protected virtual void OnPointerExit() 
    {
        if (isActive) return;
        animatedObj?.SetActive(false);
    }
    protected virtual void OnPointerDown() 
    {
        //animatedWritingObj?.SetActive(false);
    }
}
