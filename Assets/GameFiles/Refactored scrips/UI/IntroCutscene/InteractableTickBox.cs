using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableTickBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected GameObject animatedWritingObj;
    [SerializeField] protected bool isActive = false;

    protected Animator animator;

    protected virtual void OnEnable()
    {
        isActive = animatedWritingObj.activeSelf;
        animatedWritingObj.SetActive(isActive);
    }

    private void Start()
    {
        animator = animatedWritingObj.GetComponent<Animator>();
        isActive = animatedWritingObj.activeSelf;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        animatedWritingObj.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (isActive) return;
        animatedWritingObj?.SetActive(false);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isActive = !isActive;
        animatedWritingObj.SetActive(isActive);
    }
}
