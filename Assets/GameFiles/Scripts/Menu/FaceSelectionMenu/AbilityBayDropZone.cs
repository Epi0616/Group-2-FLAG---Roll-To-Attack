using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AbilityBayDropZone : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        DraggableObject item = eventData.pointerDrag.GetComponent<DraggableObject>();
        item.transform.SetParent(transform);

        RectTransform itemRect = item.GetComponent<RectTransform>();
        itemRect.anchoredPosition = Vector2.zero;
    }
}
