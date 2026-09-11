using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static event Action<AbilityDropZoneParent> overAbilitySlot;

    private Canvas canvas;
    public CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    private AbilityDropZoneParent[] dropZones;
    private AbilityDropZoneParent currentParent, parentAtStartOfDrag;
    private Vector2 anchoredPositionAtStartOfDrag;

    private float dropZoneCheckInterval = 0.1f;
    private float timer = 0;
    private Coroutine checkForHighlightRoutine;

    protected virtual void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDrag(eventData);
        checkForHighlightRoutine = StartCoroutine(CheckForHighlightZone());

        parentAtStartOfDrag = currentParent;
        anchoredPositionAtStartOfDrag = rectTransform.anchoredPosition;
        transform.SetParent(canvas.transform); //so the object will be rendered infront of the drop zone while moving around.
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        OnDrag(eventData);

        PointerEventData pointerData = eventData;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);

        transform.position = canvas.transform.TransformPoint(position);
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        OnEndDrag(eventData);
        
        //Debug.Log("end drag");
        //ResetCurrentParent();
        currentParent.RemoveChild(this);

        if (checkForHighlightRoutine != null)
        {
            StopCoroutine(checkForHighlightRoutine);
        }
        overAbilitySlot?.Invoke(null);

        AbilityDropZoneParent zone = CheckForDropZone();
        if (zone != null)
        {
            if (zone.TryAddChild(this))
            {
                return;
            }
        }

        currentParent.TryAddChild(this);
    }

    private AbilityDropZoneParent CheckForDropZone()
    {
        SearchForDropZones();
        foreach (var zone in dropZones)
        {
            if (IsOverlapping(rectTransform, zone.GetComponent<RectTransform>()))
            {
                return zone.GetComponent<AbilityDropZoneParent>();
            }
        }

        return null;
    }

    private IEnumerator CheckForHighlightZone()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > dropZoneCheckInterval)
            {
                AbilityDropZoneParent zone = CheckForDropZone();
                overAbilitySlot?.Invoke(zone);

                timer = 0;
            }
            yield return null;
        }
    }

    protected virtual void OnBeginDrag(PointerEventData eventData) { }
    protected virtual void OnDrag(PointerEventData eventData) { }
    protected virtual void OnEndDrag(PointerEventData eventData) { }

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

    public Vector2 GetAnchoredPositionAtStartOfDrag()
    {
        return anchoredPositionAtStartOfDrag;
    }
    public AbilityDropZoneParent GetParentAtStartOfDrag()
    {
        return parentAtStartOfDrag;
    }
    public void SetCurrentParent(AbilityDropZoneParent newParent)
    { 
        currentParent = newParent;
    }
    public AbilityDropZoneParent GetCurrentParent()
    { 
        return currentParent;
    }

    public void ResetCurrentParent()
    {
        if (!currentParent) { return; }
        currentParent.RemoveChild(this);
        currentParent = null;
    }

    public void SearchForDropZones()
    {
        dropZones = FindObjectsByType<AbilityDropZoneParent>(FindObjectsSortMode.None);
    }
}