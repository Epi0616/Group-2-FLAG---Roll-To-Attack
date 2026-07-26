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
        animatedObj?.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (isActive) return;
        animatedObj?.SetActive(false);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //animatedWritingObj?.SetActive(false);
    }
}
