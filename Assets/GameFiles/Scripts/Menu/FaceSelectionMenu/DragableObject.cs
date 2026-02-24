using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Canvas canvas;
    private AbilityBayDropZone[] dropZones;
    [SerializeField] private RectTransform rectTransform;
    private Transform currentParent;

    private void Awake()
    {
        dropZones = FindObjectsByType<AbilityBayDropZone>(FindObjectsSortMode.None);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        currentParent = transform.parent;
        transform.SetParent(canvas.transform); //so the object will be rendered infront of the drop zone while moving around.
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end drag");

        foreach (var zone in dropZones)
        {
            if (IsOverlapping(rectTransform, zone.GetComponent<RectTransform>()))
            {
                Debug.Log("dropped in valid zone");
                transform.SetParent(zone.transform);
                transform.SetAsLastSibling(); //makes the object appear on top of all other objects within a drop zone.
                //rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
        }

        transform.SetParent(currentParent);
        transform.SetAsLastSibling(); //makes the object appear on top of all other objects within a drop zone.
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);

        return rectA.Overlaps(rectB);
    }

    private Rect GetWorldRect(RectTransform rectTransform) //more accurate collision checking as opposted to just getting the rect of a/b in "IsOverlapping"
    {
        Vector3[] fourCorners = new Vector3[4];
        rectTransform.GetWorldCorners(fourCorners);

        float x = fourCorners[0].x;
        float y = fourCorners[0].y;
        float width = fourCorners[2].x - fourCorners[0].x;
        float height = fourCorners[2].y - fourCorners[0].y;

        return new Rect(x, y, width, height);
    }
}
