using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableSizeScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector3 hoverScale = new(1.05f,1.05f,1.05f);
    [SerializeField] private Vector3 pressedScale = new(0.95f, 0.95f, 0.95f);
    private Vector3 startScale; //this assumes start scale is 1,1,1
    private RectTransform rectTransform;
    
    private Coroutine coroutine;
    private bool isMouseCaptured;
    private bool isMouseDown;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startScale = rectTransform.localScale;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isMouseCaptured = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, hoverScale, transform.localScale));
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isMouseCaptured = false;
        if (isMouseDown) return;

        if (coroutine != null)
        { 
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, startScale, transform.localScale));
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ScaleToFrom(0.05f, pressedScale, transform.localScale));
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
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

    private IEnumerator ScaleToFrom(float duration, Vector3 to, Vector3 from)
    {
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
