using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ControllerScrollAdjust : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private InputActionReference navigate;
    [SerializeField] private float scrollPadding;


    private GameObject lastSelected = null;


    private void OnEnable()
    {
        navigate.action.performed += OnNavigate;
    }

    private void OnDisable()
    {
        navigate.action.performed -= OnNavigate;
    }

    private void OnNavigate(InputAction.CallbackContext context)
    { 
        UpdateContentPosition();
    }

    private void UpdateContentPosition()
    {
        GameObject activeObj = EventSystem.current.currentSelectedGameObject;

        if (lastSelected == activeObj) return;
        lastSelected = activeObj;

        if (activeObj == null) return;
        if (!activeObj.transform.IsChildOf(scrollRect.content.transform)) return;

        RectTransform activeTransform = activeObj.transform as RectTransform;

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        Canvas.ForceUpdateCanvases();

        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float contentYPos = scrollRect.content.anchoredPosition.y;

        Vector3[] activeCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];

        activeTransform.GetWorldCorners(activeCorners);
        viewport.GetWorldCorners(viewportCorners);

        float activeTop = activeCorners[1].y;
        float activeBottom = activeCorners[0].y;
        float viewportTop = viewportCorners[1].y - scrollPadding;
        float viewportBottom = viewportCorners[0].y + scrollPadding;

        Debug.Log(contentYPos);
        Debug.Log(contentHeight);
        Debug.Log(viewportHeight);

        if (activeTop > viewportTop)
        {
            if (contentYPos < 0.5f) return;
            content.anchoredPosition -= new Vector2(0, activeTop - viewportTop);
        }
        else if (activeBottom < viewportBottom)
        {
            if (contentHeight - contentYPos < viewportHeight + 0.5f) return;
            content.anchoredPosition += new Vector2(0, viewportBottom - activeBottom);
        }

    }
}
