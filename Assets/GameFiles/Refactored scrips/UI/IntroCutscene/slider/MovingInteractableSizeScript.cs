using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingInteractableSizeScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Vector3 hoverScale = new(1.05f, 1.05f, 1.05f);
    [SerializeField] private Vector3 pressedScale = new(0.95f, 0.95f, 0.95f);
    private Vector3 startScale; //this assumes start scale is 1,1,1
    private RectTransform rectTransform;

    private Coroutine coroutine;
    private bool isMouseCaptured;
    private bool isMouseDown;

    private void Start()
    {
        rectTransform = GetComponentInChildren<RectTransform>();
        startScale = rectTransform.localScale;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer entered");
        isMouseCaptured = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, hoverScale, transform.localScale));
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exited");
        isMouseCaptured = false;
        if (isMouseDown) return;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, startScale, transform.localScale));
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("pointer down");
        isMouseDown = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, pressedScale, transform.localScale));
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("pointer up");
        isMouseDown = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (isMouseCaptured == false)
        {
            coroutine = StartCoroutine(ScaleToFrom(0.05f, startScale, transform.localScale));
        }
        else
        {
            coroutine = StartCoroutine(ScaleToFrom(0.05f, hoverScale, transform.localScale));
        }
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
    }

    //void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("pointer down");
    //    isMouseDown = true;
    //    if (coroutine != null)
    //    {
    //        StopCoroutine(coroutine);
    //    }
    //    coroutine = StartCoroutine(ScaleToFrom(0.05f, pressedScale, transform.localScale));
    //}

    //void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("pointer up");
    //    isMouseDown = false;
    //    if (coroutine != null)
    //    {
    //        StopCoroutine(coroutine);
    //    }
    //    if (isMouseCaptured == false)
    //    {
    //        coroutine = StartCoroutine(ScaleToFrom(0.05f, startScale, transform.localScale));
    //    }
    //    else
    //    {
    //        coroutine = StartCoroutine(ScaleToFrom(0.05f, hoverScale, transform.localScale));
    //    }
    //}

    private IEnumerator ScaleToFrom(float duration, Vector3 to, Vector3 from)
    {
        Debug.Log("adjusting scale");
        float timer = duration;
        float t = 0f;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            rectTransform.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }

        rectTransform.localScale = to;
        coroutine = null;
    }
}
