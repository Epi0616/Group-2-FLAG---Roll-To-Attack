using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas canvas;
    public CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    private AbilityDropZoneParent[] dropZones;
    private AbilityDropZoneParent currentParent;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        dropZones = FindObjectsByType<AbilityDropZoneParent>(FindObjectsSortMode.None);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform); //so the object will be rendered infront of the drop zone while moving around.
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end drag");
        ResetCurrentParent();
        foreach (var zone in dropZones)
        {
            if (IsOverlapping(rectTransform, zone.GetComponent<RectTransform>()))
            {
                zone.GetComponent<AbilityDropZoneParent>().AddChild(this);
            }
        }
    }

    private bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);

        return rectA.Overlaps(rectB);
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] fourCorners = new Vector3[4];
        rectTransform.GetWorldCorners(fourCorners);

        float x = fourCorners[0].x;
        float y = fourCorners[0].y;
        float width = fourCorners[2].x - fourCorners[0].x;
        float height = fourCorners[2].y - fourCorners[0].y;

        return new Rect(x, y, width, height);
    }

    public void SetCurrentParent(AbilityDropZoneParent newParent)
    { 
        currentParent = newParent;
    }

    private void ResetCurrentParent()
    {
        if (!currentParent) { return; }
        currentParent.RemoveChild(this);
        currentParent = null;
    }
}
