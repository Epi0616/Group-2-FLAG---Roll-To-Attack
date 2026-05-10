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

    [SerializeField] private GameObject gamePadFirstSelected;
    [SerializeField] private GameObject contentGamepad, contentKeyboard;
    private RectTransform gamepadTransform, keyboardTransform;

    private GameObject lastSelected = null;


    private void OnEnable()
    {
        SettingsUIManager.keyBindUIOpened += SetFirstElement;
        UISelectionManager.switchToGamepad += SwitchToGamepad;
        UISelectionManager.switchToKeyboard += SwitchToKeyboard;
        navigate.action.performed += OnNavigate;

        UpdateControlScheme();
    }

    private void OnDisable()
    {
        SettingsUIManager.keyBindUIOpened -= SetFirstElement;
        UISelectionManager.switchToGamepad -= SwitchToGamepad;
        UISelectionManager.switchToKeyboard -= SwitchToKeyboard;    
        navigate.action.performed -= OnNavigate;
    }

    private void Awake()
    {
        gamepadTransform = contentGamepad.GetComponent<RectTransform>();
        keyboardTransform = contentKeyboard.GetComponent<RectTransform>();
        SwitchToKeyboard();
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

    private void UpdateControlScheme()
    {
        if (UISelectionManager.instance.isGamepadActive)
        {
            SwitchToGamepad();
            return;
        }

        SwitchToKeyboard();
    }

    public void SetFirstElement()
    {         
        if (scrollRect.content.childCount == 0) return;
        UISelectionManager.instance.TrySetSelectedGameObject(gamePadFirstSelected);
        EventSystem.current.firstSelectedGameObject = gamePadFirstSelected;
    }

    private void SwitchToGamepad()
    {
        contentKeyboard.SetActive(false);
        contentGamepad.SetActive(true);
        SetFirstElement();

        scrollRect.content = gamepadTransform;
    }

    private void SwitchToKeyboard()
    {
        contentGamepad.SetActive(false);
        contentKeyboard.SetActive(true);

        scrollRect.content = keyboardTransform;
    }
}
